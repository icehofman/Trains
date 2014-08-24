using Trains.Plan;

namespace Trains.Specify
{
    public class OriginAndDestinationSpecification : IRouteSpecification
    {
        /// <summary>
        /// The origin city name
        /// </summary>
        private string originName;

        /// <summary>
        /// The destination city name
        /// </summary>
        private string destinationName;

        /// <summary>
        /// Initializes a new instance of the <see cref="OriginAndDestinationSpecification"/> class.
        /// </summary>
        /// <param name="origin">The origin.</param>
        /// <param name="destination">The destination.</param>
        public OriginAndDestinationSpecification(string origin, string destination)
        {
            this.originName = origin;
            this.destinationName = destination;
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
            return route.Destination.Name == this.destinationName && route.Origin.Name == this.originName;
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
            return route.Origin.Name == this.originName;
        }

        #endregion
    }
}
