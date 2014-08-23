using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trains.Map;
using Trains.Plan;

namespace Trains.Specify
{
    public class PathSpecification : IRouteSpecification
    {
        /// <summary>
        /// The city route to specify
        /// </summary>
        private string[] citiesRoute;

        /// <summary>
        /// Initializes a new instance of the <see cref="PathSpecification"/> class.
        /// </summary>
        /// <param name="cityNames">The city names.</param>
        public PathSpecification(params string[] cityNames)
        {
            this.citiesRoute = cityNames;
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
            IEnumerable<IRailroad> legs = route.Legs;

            if (legs.Count() + 1 != this.citiesRoute.Length)
            {
                return false;
            }

            for (int i = 0; i < legs.Count(); i++)
            {
                if (legs.ElementAt(i).Origin.Name != this.citiesRoute[i] ||
                   legs.ElementAt(i).Destination.Name != this.citiesRoute[i + 1])
                {
                    return false;
                }
            }

            return true;
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
            int i = 0;
            if (route.Legs.Count() > this.citiesRoute.Length)
            {
                return false;
            }

            foreach (IRailroad leg in route.Legs)
            {
                if (leg.Origin.Name != this.citiesRoute[i] ||
                   leg.Destination.Name != this.citiesRoute[i + 1])
                {
                    return false;
                }

                i++;
            }

            return true;
        }

        #endregion
    }
}
