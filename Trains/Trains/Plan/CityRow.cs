using System.Collections.Generic;
using System.Linq;

namespace Trains.Plan
{
    public class CityRow
    {
        /// <summary>
        /// The data about the neighbors to this city
        /// </summary>
        private readonly Dictionary<string, int> neighborData = new Dictionary<string, int>();

        /// <summary>
        /// Initializes a new instance of the <see cref="CityRow"/> class.
        /// </summary>
        /// <param name="rowCity">The row's city.</param>
        /// <param name="cities">The cities.</param>
        public CityRow(string rowCity, string[] cities)
        {
            this.Name = rowCity;
            foreach (string city in cities)
            {
                this.neighborData.Add(city, ShortestLengthFinder.Unreachable);
            }
        }

        /// <summary>
        /// Gets the name of the city of the row.
        /// </summary>
        public string Name
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets or sets the distance <see cref="System.Int32"/> to the specified destination.
        /// </summary>
        /// <param name="destination">The name of the destination city</param>
        /// <returns>
        /// The length from this city to the city with the given name
        /// </returns>
        public int this[string destination]
        {
            get
            {
                return this.neighborData[destination];
            }

            set
            {
                this.neighborData[destination] = value;
            }
        }

        /// <summary>
        /// Gets or sets the distance <see cref="System.Int32"/> to the destination in the specified index.
        /// </summary>
        /// <param name="index">The index of the destination city</param>
        /// <returns>
        /// The length from this city to the city in the given index
        /// </returns>
        public int this[int index]
        {
            get
            {
                return this.neighborData[this.neighborData.Keys.ElementAt(index)];
            }

            set
            {
                this.neighborData[this.neighborData.Keys.ElementAt(index)] = value;
            }
        }
    }
}
