using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Trains.Plan
{
    public interface IRouteFinder
    {
        /// <summary>
        /// Finds the routes that satisfy a specification
        /// </summary>
        /// <param name="specification">The specification to satisfy.</param>
        /// <returns>
        /// The routes that satisfy the specified attributes
        /// </returns>
        IEnumerable<IRoute> FindRoutes(IRouteSpecification specification);

        /// <summary>
        /// Finds the first satisfying route to the specification.
        /// </summary>
        /// <param name="specification">The specification to satisfy.</param>
        /// <returns>
        /// The first route to satisfy the previously specified attributes
        /// </returns>
        IRoute FindFirstSatisfyingRoute(IRouteSpecification specification);
    }
}
