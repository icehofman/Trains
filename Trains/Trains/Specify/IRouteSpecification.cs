using Trains.Plan;

namespace Trains.Specify
{
    public interface IRouteSpecification
    {
        /// <summary>
        /// Validates the specified the object.
        /// </summary>
        /// <param name="route">The object to validate with this specification.</param>
        /// <returns>true if the object conforms to this specification</returns>
        bool IsSatisfiedBy(IRoute route);

        /// <summary>
        /// Determines if the route might satisfy the RouteSpecification
        /// </summary>
        /// <param name="route">The route.</param>
        /// <returns>true if it might satisfy this specification, false if there's no way.</returns>
        bool MightBeSatisfiedBy(IRoute route);
    }
}
