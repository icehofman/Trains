using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trains.Plan;

namespace Trains.Specify
{
    public class AndSpecification : IRouteSpecification
    {
        /// <summary>
        /// The specifications that will be AND connected.
        /// </summary>
        private IRouteSpecification[] specifications;

        /// <summary>
        /// Initializes a new instance of the <see cref="AndSpecification"/> class.
        /// </summary>
        /// <param name="specifications">The specifications to connect with AND operator.</param>
        public AndSpecification(params IRouteSpecification[] specifications)
        {
            this.specifications = specifications;
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
            foreach (IRouteSpecification specification in this.specifications)
            {
                if (!specification.IsSatisfiedBy(route))
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
            foreach (IRouteSpecification specification in this.specifications)
            {
                if (!specification.MightBeSatisfiedBy(route))
                {
                    return false;
                }
            }

            return true;
        }

        #endregion
    }
}
