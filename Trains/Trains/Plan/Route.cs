using System.Collections.Generic;
using Trains.Map;

namespace Trains.Plan
{
    public class Route : IRoute
    {
        /// <summary>
        /// The legs that make up this Route
        /// </summary>
        private IList<IRailroad> legs;

        /// <summary>
        /// Initializes a new instance of the <see cref="Route"/> class.
        /// </summary>
        public Route()
        {
            this.legs = new List<IRailroad>();
        }

        #region IRoute Members

        /// <summary>
        /// Gets the legs that conform the route.
        /// </summary>
        /// <value>
        /// The legs that conform the route.
        /// </value>
        public IEnumerable<IRailroad> Legs
        {
            get
            {
                return this.legs;
            }
        }

        /// <summary>
        /// Gets the total distance.
        /// </summary>
        public int Distance
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the origin.
        /// </summary>
        public ICity Origin
        {
            get
            {
                return this.legs[0].Origin;
            }
        }

        /// <summary>
        /// Gets the destination.
        /// </summary>
        public ICity Destination
        {
            get
            {
                return this.legs[this.legs.Count - 1].Destination;
            }
        }

        /// <summary>
        /// Adds a leg to the route.
        /// </summary>
        /// <param name="railroad">The railroad leg to be added.</param>
        public void AddLeg(IRailroad railroad)
        {
            this.Distance += railroad.Length;
            this.legs.Add(railroad);
        }

        /// <summary>
        /// Creates a fly weight copy of this instance.
        /// </summary>
        /// <returns>
        /// A fly weight copy
        /// </returns>
        public IRoute FlyweightCopy()
        {
            Route flyCopy = new Route
            {
                legs = new List<IRailroad>(this.legs),
                Distance = this.Distance
            };

            return flyCopy;
        }
        #endregion

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            string graph = "{";

            foreach (IRailroad leg in this.legs)
            {
                graph += " " + leg.Origin.Name + leg.Destination.Name + leg.Length;
            }

            return graph + " }";
        }

        /// <summary>
        /// Determines whether the specified <see cref="System.Object"/> is equal to this instance.
        /// </summary>
        /// <param name="obj">The <see cref="System.Object"/> to compare with this instance.</param>
        /// <returns>
        ///   <c>true</c> if the specified <see cref="System.Object"/> is equal to this instance; otherwise, <c>false</c>.
        /// </returns>
        public override bool Equals(object obj)
        {
            Route other = obj as Route;
            if (other != null)
            {
                return this.ToString().Equals(other.ToString());
            }

            return base.Equals(obj);
        }

        /// <summary>
        /// Returns a hash code for this instance.
        /// </summary>
        /// <returns>
        /// A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table. 
        /// </returns>
        public override int GetHashCode()
        {
            return this.ToString().GetHashCode();
        }
    }
}
