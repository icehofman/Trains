using System.Collections.Generic;
using Trains.Map;

namespace Trains.Plan
{
    public interface IRoute
    {
        /// <summary>
        /// Gets the legs that conform the route.
        /// </summary>
        /// <value>
        /// The legs that conform the route.
        /// </value>
        IEnumerable<IRailroad> Legs
        {
            get;
        }

        /// <summary>
        /// Gets the total distance.
        /// </summary>
        int Distance
        {
            get;
        }

        /// <summary>
        /// Gets the origin.
        /// </summary>
        ICity Origin
        {
            get;
        }

        /// <summary>
        /// Gets the destination.
        /// </summary>
        ICity Destination
        {
            get;
        }

        /// <summary>
        /// Adds the railroad leg stop.
        /// NOTE: Legs can repeat, as long as they are not continuous.
        /// </summary>
        /// <param name="railroad">The railroad to add.</param>
        void AddLeg(IRailroad railroad);

        /// <summary>
        /// Creates a flyweight copy of this instance.
        /// </summary>
        /// <returns>A fly weight copy</returns>
        IRoute FlyweightCopy();
    }
}
