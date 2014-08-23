using NSubstitute;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using Trains.Map;
using Trains.Plan;

namespace Trains.Test
{
    public delegate object MethodCall(params object[] arguments);
    public static class TestHelper
    {
        /// <summary>
        /// Private instance binding flags
        /// </summary>
        private const BindingFlags PRIVATE = BindingFlags.Instance | BindingFlags.NonPublic;

        /// <summary>
        /// Private static binding flags
        /// </summary>
        private const BindingFlags PRIVATE_STATIC = BindingFlags.NonPublic | BindingFlags.Static;

        /// <summary>
        /// Gets a private static method.
        /// </summary>
        /// <typeparam name="T">Type reflect</typeparam>
        /// <param name="methodName">Name of the method.</param>
        /// <returns>A MethodCall delegate</returns>        
        public static MethodCall GetPrivateStaticMethod<T>(string methodName)
        {
            MethodInfo methodInfo = typeof(T).GetMethod(methodName, PRIVATE_STATIC);
            return parameters => methodInfo.Invoke(null, parameters);
        }

        /// <summary>
        /// Gets the public static method.
        /// </summary>
        /// <typeparam name="T">The type to reflet on for the method name</typeparam>
        /// <param name="methodName">Name of the method.</param>
        /// <returns>A MethodCall delegate for the method found.</returns>        
        public static MethodCall GetPublicStaticMethod<T>(string methodName)
        {
            MethodInfo methodInfo = typeof(T).GetMethod(methodName);
            return parameters => methodInfo.Invoke(null, parameters);
        }

        /// <summary>
        /// Gets a private method.
        /// </summary>
        /// <typeparam name="T">The type to reflect</typeparam>
        /// <param name="obj">The object to receive the invocation.</param>
        /// <param name="methodName">Name of the method.</param>
        /// <returns>The value returned by the method</returns>
        public static MethodCall GetPrivateMethod<T>(this T obj, string methodName)
        {
            return parameters => typeof(T).GetMethod(methodName, PRIVATE).Invoke(obj, parameters);
        }

        /// <summary>
        /// Gets the private field.
        /// </summary>
        /// <typeparam name="T">type of the field</typeparam>
        /// <param name="obj">The object to reflect.</param>
        /// <param name="fieldName">Name of the field.</param>
        /// <returns>The value of the field</returns>
        public static T GetPrivateField<T>(this object obj, string fieldName)
        {
            FieldInfo fieldInfo = obj.GetType().GetField(fieldName, PRIVATE);
            return (T)fieldInfo.GetValue(obj);
        }

        /// <summary>
        /// Generates the legs using the routeConfiguration.
        /// </summary>
        /// <param name="routeConfiguration">The route configuration.</param>
        /// <param name="cities">The preconfigured cities.</param>
        /// <returns>
        /// The legs configured
        /// </returns>
        public static IList<IRailroad> GenerateLegs(string routeConfiguration, IList<ICity> cities = null)
        {
            return GenerateLegs(routeConfiguration.Split(' ', ','), cities);
        }

        /// <summary>
        /// Generates the legs using the routeConfiguration.
        /// </summary>
        /// <param name="routeConfiguration">The route configuration.</param>
        /// <param name="cities">The preconfigured cities.</param>
        /// <returns>
        /// The legs configured
        /// </returns>
        public static IList<IRailroad> GenerateLegs(string[] routeConfiguration, IList<ICity> cities = null)
        {
            IList<IRailroad> legs = new List<IRailroad>();
            if (cities == null)
            {
                cities = GenerateCities(routeConfiguration);
            }

            foreach (string railroadConfiguration in routeConfiguration)
            {
                string originName = railroadConfiguration[0].ToString();
                string destinationName = railroadConfiguration[1].ToString();
                ICity originCity = cities.First(city => city.Name.Equals(originName));
                ICity destinationCity = cities.First(city => city.Name.Equals(destinationName));
                IRailroad railroad = BuildRailroad(railroadConfiguration, originCity, destinationCity);
                legs.Add(railroad);
                IList<IRailroad> outgoingRailroads = originCity.Outgoing;
                outgoingRailroads.Add(railroad);
            }

            return legs;
        }

        /// <summary>
        /// Generates the cities using the graph configuration.
        /// </summary>
        /// <param name="graphConfiguration">The graph configuration.</param>
        /// <param name="addRailroads">if set to <c>true</c> [add railroads].</param>
        /// <returns>
        /// The cities identified
        /// </returns>
        public static IList<ICity> GenerateCities(string graphConfiguration, bool addRailroads = false)
        {
            return GenerateCities(graphConfiguration.Split(' ', ','), addRailroads);
        }

        /// <summary>
        /// Generates the cities using the route configuration.
        /// </summary>
        /// <param name="routeConfiguration">The route configuration.</param>
        /// <param name="addRailroads">if set to <c>true</c> [add railroads].</param>
        /// <returns>
        /// The cities identified
        /// </returns>
        public static IList<ICity> GenerateCities(string[] routeConfiguration, bool addRailroads = false)
        {
            IList<ICity> cities = new List<ICity>();

            foreach (string railroadConfiguration in routeConfiguration)
            {
                string originCityName = railroadConfiguration[0].ToString();
                string destinationCityName = railroadConfiguration[1].ToString();
                ICity origin = AddIfNotIncluded(cities, originCityName);
                ICity destination = AddIfNotIncluded(cities, destinationCityName);

                if (addRailroads)
                {
                    IRailroad built = BuildRailroad(railroadConfiguration, origin, destination);
                    origin.Outgoing.Add(built);
                }
            }

            return cities;
        }

        /// <summary>
        /// Builds the route from a route path string.
        /// Graph examples: [AB1 BC1] or [AB1,BC1] or [AB1, BC1] all are good.
        /// </summary>
        /// <param name="routePath">The route path.</param>
        /// <returns>The resulting route.</returns>
        public static Route BuildRouteFromString(string routePath)
        {
            Route route = new Route();
            string[] railroadsConfigs = routePath.Split(new string[] { ", ", " ", "," }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string railroadConfig in railroadsConfigs)
            {
                string originCityName = railroadConfig.Substring(0, 1);
                string destinationCityName = railroadConfig.Substring(1, 1);
                int legLength = int.Parse(railroadConfig.Substring(2));
                City originCity = GetOrCreateCity(route, originCityName);
                City destinationCity = GetOrCreateCity(route, destinationCityName);
                Railroad railroad = new Railroad()
                {
                    Origin = originCity,
                    Destination = destinationCity,
                    Length = legLength
                };

                route.AddLeg(railroad);
            }

            return route;
        }

        /// <summary>
        /// Builds the mock route.
        /// </summary>
        /// <param name="specifiedRoute">The specified route.</param>
        /// <returns>The build route</returns>
        public static IRoute BuildMockRoute(string specifiedRoute)
        {
            return BuildMockRoute(specifiedRoute.Split(' ', ','));
        }

        /// <summary>
        /// Builds a mock IRoute.
        /// </summary>
        /// <param name="specifiedRoute">The specified route.</param>
        /// <returns>The built route.</returns>
        public static IRoute BuildMockRoute(string[] specifiedRoute)
        {
            IRoute routeSpec = Substitute.For<IRoute>();
            IList<IRailroad> specifiedLegs = TestHelper.GenerateLegs(specifiedRoute);
            routeSpec.Legs.ReturnsForAnyArgs(specifiedLegs);

            // Specify distance
            int distance = specifiedLegs.Sum(leg => leg.Length);
            routeSpec.Distance.ReturnsForAnyArgs(distance);

            // Specify origin
            ICity origin = specifiedLegs.First().Origin;
            routeSpec.Origin.ReturnsForAnyArgs(origin);

            // Specify destination
            ICity destination = specifiedLegs.Last().Destination;
            routeSpec.Destination.ReturnsForAnyArgs(destination);

            return routeSpec;
        }

        /// <summary>
        /// Builds the railroad.
        /// </summary>
        /// <param name="railroadConfiguration">The railroad configuration.</param>
        /// <param name="originCity">The origin city.</param>
        /// <param name="destinationCity">The destination city.</param>
        /// <returns>The railroad</returns>
        private static IRailroad BuildRailroad(string railroadConfiguration, ICity originCity, ICity destinationCity)
        {
            IRailroad railroad = Substitute.For<IRailroad>();
            if (railroadConfiguration.Length > 2)
            {
                railroad.Length.ReturnsForAnyArgs(int.Parse(railroadConfiguration.Substring(2)));
            }

            railroad.Origin.ReturnsForAnyArgs(originCity);
            railroad.Destination.ReturnsForAnyArgs(destinationCity);

            return railroad;
        }

        /// <summary>
        /// Adds a city with the <paramref name="originCityName"/> if not included already in the city list.
        /// </summary>
        /// <param name="cities">The city list.</param>
        /// <param name="originCityName">Name of the city.</param>
        /// <returns>The city added or retrieved</returns>
        private static ICity AddIfNotIncluded(IList<ICity> cities, string originCityName)
        {
            ICity city = cities.FirstOrDefault(citi => citi.Name.Equals(originCityName));
            if (city == default(ICity))
            {
                city = Substitute.For<ICity>();
                city.Name.Returns(originCityName);
                IList<IRailroad> railroadList = new List<IRailroad>();
                city.Outgoing.ReturnsForAnyArgs(railroadList);
                cities.Add(city);
            }

            return city;
        }

        /// <summary>
        /// Gets the city. Null if it is not found.
        /// </summary>
        /// <param name="route">The route on which to look.</param>
        /// <param name="cityName">Name of the city.</param>
        /// <returns>The city with the city name, null if not found.</returns>
        private static City GetOrCreateCity(Route route, string cityName)
        {
            City city = route.Legs.SelectMany(leg => new ICity[] { leg.Origin, leg.Destination })
                             .FirstOrDefault(item => item.Name.Equals(cityName)) as City;

            if (city == null)
            {
                city = new City()
                {
                    Name = cityName
                };
            }

            return city;
        }
    }
}
