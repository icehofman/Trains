using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trains.Plan
{
    public interface IRouteComparer
    {
        /// <summary>
        /// Starts a new comparison for the route.
        /// </summary>
        /// <param name="route">The route.</param>
        /// <returns>The new comparison.</returns>
        IRouteComparison Is(IRoute route);
    }
}
