namespace Trains.Plan
{
    public interface IRouteComparison
    {
        /// <summary>
        /// Determines if a given route is betters the than another.
        /// </summary>
        /// <param name="route">The route.</param>
        /// <returns>true if the first route is better than the one received, false otherwise.</returns>
        bool BetterThan(IRoute route);

        /// <summary>
        /// Determins if a given route is worse than another.
        /// </summary>
        /// <param name="route">The route.</param>
        /// <returns>true if the given route is worse than the one received.</returns>
        bool WorseThan(IRoute route);
    }
}
