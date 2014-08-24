using System.Linq;
using Trains.Plan;

namespace Trains.Specify
{
    public class StopsCountSpecification : IRouteSpecification
    {
        /// <summary>
        /// The maximum number of stops specified
        /// </summary>
        private int maxStopsCount;

        /// <summary>
        /// The minimum number of stops specified
        /// </summary>
        private int minStopsCount;

        /// <summary>
        /// Initializes a new instance of the <see cref="StopsCountSpecification"/> class.
        /// </summary>
        /// <param name="minStops">The minimum number of stops.</param>
        /// <param name="maxStops">The maximum number of stops.</param>
        public StopsCountSpecification(int minStops, int maxStops)
        {
            this.maxStopsCount = maxStops;
            this.minStopsCount = minStops;
        }

        #region IRouteSpecification Members

        /// <summary>
        /// Validates the specified the object.
        /// </summary>
        /// <param name="route">The object to validate with this specification.</param>
        /// <returns>
        /// true if the object conforms to this specification
        /// </returns>
        public bool IsSatisfiedBy(IRoute route)
        {
            int legsCount = route.Legs.Count();
            return legsCount <= this.maxStopsCount && legsCount >= this.minStopsCount;
        }

        /// <summary>
        /// Determines if the route might satisfy the RouteSpecification
        /// </summary>
        /// <param name="route">The route.</param>
        /// <returns>
        /// true if it might satisfy this specification, false if there's no way.
        /// </returns>
        public bool MightBeSatisfiedBy(IRoute route)
        {
            int legsCount = route.Legs.Count();
            return legsCount <= this.maxStopsCount;
        }

        #endregion
    }
}
