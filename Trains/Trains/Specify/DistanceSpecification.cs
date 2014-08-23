using Trains.Plan;

namespace Trains.Specify
{
    public class DistanceSpecification : IRouteSpecification
    {
        /// <summary>
        /// The minimum total distance specified for a route
        /// </summary>
        private int minDistance;

        /// <summary>
        /// The maximum total distance specified for a route
        /// </summary>
        private int maxDistance;

        /// <summary>
        /// Initializes a new instance of the <see cref="DistanceSpecification"/> class.
        /// </summary>
        /// <param name="minDistance">The min route distance.</param>
        /// <param name="maxDistance">The max route distance.</param>
        public DistanceSpecification(int minDistance, int maxDistance)
        {
            this.minDistance = minDistance;
            this.maxDistance = maxDistance;
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
            return route.Distance <= this.maxDistance && route.Distance >= this.minDistance;
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
            return route.Distance <= this.maxDistance;
        }

        #endregion
    }
}
