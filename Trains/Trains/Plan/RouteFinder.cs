using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trains.Map;
using Trains.Specify;

namespace Trains.Plan
{
    public class RouteFinder : IRouteFinder
    {
        /// <summary>
        /// It is the railroad map used to find the route.
        /// </summary>
        private IRailroadMap map;

        /// <summary>
        /// Initializes a new instance of the <see cref="RouteFinder"/> class.
        /// </summary>
        /// <param name="map">The map.</param>
        public RouteFinder(IRailroadMap map)
        {
            this.map = map;
        }

        #region IRouteFinder Members

        /// <summary>
        /// Finds the routes that satisfy a specification
        /// </summary>
        /// <param name="specification">The specification to satisfy.</param>
        /// <returns>
        /// The routes that satisfy the specified attributes
        /// </returns>
        public IEnumerable<IRoute> FindRoutes(IRouteSpecification specification)
        {
            foreach (ICity rootCity in this.map.Cities.Where(city => city.Outgoing != null && city.Outgoing.Count > 0))
            {
                foreach (var railroad in rootCity.Outgoing)
                {
                    IRoute rootRoute = new Route();
                    rootRoute.AddLeg(railroad);
                    IEnumerable<IRoute> results = FindSatisfyingRoutes(rootRoute, specification);
                    foreach (var result in results)
                    {
                        yield return result;
                    }
                }
            }
        }

        /// <summary>
        /// Finds the first satisfying route to the specification.
        /// </summary>
        /// <param name="specification">The specification to satisfy.</param>
        /// <returns>
        /// The first route to satisfy the previously specified attributes
        /// </returns>
        public IRoute FindFirstSatisfyingRoute(IRouteSpecification specification)
        {
            foreach (ICity city in this.map.Cities.Where(city => city.Outgoing != null && city.Outgoing.Count > 0))
            {
                foreach (IRailroad railroad in city.Outgoing)
                {
                    IRoute root = new Route();
                    root.AddLeg(railroad);
                    IRoute result = FindFirstSample(root, specification);
                    if (result != default(IRoute))
                    {
                        return result;
                    }
                }
            }

            return default(IRoute);
        }

        #endregion

        /// <summary>
        /// Finds the satisfying routes.
        /// </summary>
        /// <param name="route">The current route state.</param>
        /// <param name="specification">The specification to satisfy.</param>
        /// <returns>All sub-routes that satisfy the specification</returns>
        private static IEnumerable<IRoute> FindSatisfyingRoutes(IRoute route, IRouteSpecification specification)
        {
            if (specification.MightBeSatisfiedBy(route))
            {
                if (specification.IsSatisfiedBy(route))
                {
                    yield return route;
                }

                // Even if a rounte already satisfies a condition, we must keep on looking for more,
                // unil no more routes could possibly satisfy.
                ICity currentDestination = route.Destination;
                foreach (IRailroad nextRailroad in currentDestination.Outgoing)
                {
                    IRoute nextRoute = route.FlyweightCopy();
                    nextRoute.AddLeg(nextRailroad);
                    foreach (IRoute found in FindSatisfyingRoutes(nextRoute, specification))
                    {
                        yield return found;
                    }
                }
            }
        }

        /// <summary>
        /// Finds the first sample that satisfies the specification.
        /// </summary>
        /// <param name="route">The current route state.</param>
        /// <param name="specification">The specification to satisfy.</param>
        /// <returns>
        /// The first city to satisfy the specification.
        /// </returns>
        private static IRoute FindFirstSample(IRoute route, IRouteSpecification specification)
        {
            if (specification.MightBeSatisfiedBy(route))
            {
                if (specification.IsSatisfiedBy(route))
                {
                    return route;
                }
                else
                {
                    ICity currentDestination = route.Destination;
                    IRoute sample = default(IRoute);
                    foreach (IRailroad nextRailroad in currentDestination.Outgoing)
                    {
                        IRoute nextRoute = route.FlyweightCopy();
                        nextRoute.AddLeg(nextRailroad);
                        sample = FindFirstSample(nextRoute, specification);
                        if (sample != default(IRoute))
                        {
                            return sample;
                        }
                    }
                }
            }

            return default(IRoute);
        }
    }
}
