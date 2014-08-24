using System.Collections.Generic;

namespace Trains.Map
{
    public class City : ICity
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="City"/> class.
        /// </summary>
        public City()
        {
            this.Outgoing = new List<IRailroad>();
        }

        /// <summary>
        /// Gets or sets the name of the city.
        /// </summary>
        /// <value>
        /// The name of the city.
        /// </value>
        public string Name
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the outgoing railroads.
        /// </summary>
        /// <value>
        /// The outgoing railroads.
        /// </value>
        public IList<IRailroad> Outgoing
        {
            get;
            private set;
        }
    }
}
