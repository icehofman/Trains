using System.Linq;
using Trains.Map;
using Trains.Plan;

namespace Trains.Test.Plan
{
    public static class FinderTestHelper
    {
        /// <summary>
        /// It represents a city specification to ignore.
        /// </summary>
        public const string IGNORED_CITY = "Z";

        /// <summary>
        /// It represents an ignored railroad length.
        /// </summary>
        public const int IGNORE_DISTANCE = 0;

        /// <summary>
        /// It represents the count of ignored route legs specification
        /// </summary>
        public const int IGNORE_LEGS_COUNT = 0;

        /// <summary>
        /// It represents Route legs to ignore in the specification.
        /// </summary>
        public static readonly IRailroad[] IGNORE_LEGS = new IRailroad[] { };

        /// <summary>
        /// Determines if a route might satisfy a specification
        /// </summary>
        /// <param name="routeSpec">The route spec.</param>
        /// <param name="route">The route.</param>
        /// <returns>true if it might satisfy the specification, false otherwise</returns>
        public static bool MightSatisfySpecification(IRoute routeSpec, IRoute route)
        {
            if (routeSpec.Distance > IGNORE_DISTANCE && route.Distance > routeSpec.Distance)
            {
                return false;
            }

            if (routeSpec.Legs != IGNORE_LEGS && routeSpec.Legs.Count() != IGNORE_LEGS_COUNT && route.Legs.Count() > routeSpec.Legs.Count())
            {
                return false;
            }

            if (!SatisfiesSpecification(routeSpec.Origin, route.Origin))
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// Determines if a route satisfies a specification
        /// </summary>
        /// <param name="routeSpec">The route specification.</param>
        /// <param name="route">The route to compare.</param>
        /// <returns>true if it satisfies the specification, false otherwise.</returns>
        public static bool SatisfiesSpecification(IRoute routeSpec, IRoute route)
        {
            if (!SatisfiesSpecification(routeSpec.Destination, route.Destination) || !SatisfiesSpecification(routeSpec.Origin, route.Origin))
            {
                return false;
            }

            if (routeSpec.Legs != IGNORE_LEGS && routeSpec.Legs.Count() != IGNORE_LEGS_COUNT)
            {
                if (routeSpec.Legs.Count() != route.Legs.Count())
                {
                    return false;
                }

                for (int i = 0; i < routeSpec.Legs.Count(); i++)
                {
                    IRailroad specRailroad = routeSpec.Legs.ElementAt(i);
                    IRailroad routeRailroad = route.Legs.ElementAt(i);

                    if (!SatisfiesSpecification(specRailroad, routeRailroad))
                    {
                        return false;
                    }
                }
            }

            return true;
        }

        /// <summary>
        /// Determines if a railroad statisfies a specification
        /// </summary>
        /// <param name="specRailroad">The specified railroad.</param>
        /// <param name="routeRailroad">The actual railroad.</param>
        /// <returns>true if it satisfies the specification, false otherwise.</returns>
        private static bool SatisfiesSpecification(IRailroad specRailroad, IRailroad routeRailroad)
        {
            return SatisfiesSpecification(specRailroad.Origin, routeRailroad.Origin) && SatisfiesSpecification(specRailroad.Destination, routeRailroad.Destination);
        }

        /// <summary>
        /// Determines if a city satisfies a specification
        /// </summary>
        /// <param name="specifiedCity">The specified city.</param>
        /// <param name="city">The city to check.</param>
        /// <returns>true if it satisfies the specification.</returns>
        private static bool SatisfiesSpecification(ICity specifiedCity, ICity city)
        {
            return specifiedCity.Name == IGNORED_CITY || specifiedCity.Name == city.Name;
        }
    }
}
