using System;
using System.Collections.Generic;
using System.Linq;
using Trains.Map;

namespace Trains.Plan
{
    public class ShortestLengthFinder
    {
        /// <summary>
        /// Means that the distance between two points is not possible.
        /// </summary>
        public const int Unreachable = int.MaxValue >> 2;

        /// <summary>
        /// Contains the city data in rows
        /// </summary>
        private readonly IList<CityRow> cityData = new List<CityRow>();

        /// <summary>
        /// Determines if this instance has been pre-calculated
        /// </summary>
        private bool instanceInitialized;

        /// <summary>
        /// Initializes a new instance of the <see cref="ShortestLengthFinder"/> class.
        /// </summary>
        /// <param name="map">The map.</param>
        public ShortestLengthFinder(IRailroadMap map)
        {
            this.Init(map);
        }

        /// <summary>
        /// Gets the length of the shortest path from a city to another.
        /// </summary>
        /// <param name="from">Starting city.</param>
        /// <param name="to">Destination city.</param>
        /// <returns>The shortest length of the path between those cities.</returns>
        public int GetShortestLength(string from, string to)
        {
            if (!this.instanceInitialized)
            {
                this.CalculatePaths();
                this.instanceInitialized = true;
            }

            int originIndex = this.GetCityIndex(from);
            int destinationIndex = this.GetCityIndex(to);

            if (originIndex == -1 || destinationIndex == -1)
            {
                return Unreachable;
            }

            return this.cityData[originIndex][destinationIndex];
        }

        /// <summary>
        /// Initializes this instance with appropiate data
        /// </summary>
        /// <param name="map">The map.</param>
        protected void Init(IRailroadMap map)
        {
            string[] cityNames = map.Cities.Select(city => city.Name).ToArray();
            foreach (ICity city in map.Cities)
            {
                var row = new CityRow(city.Name, cityNames);
                foreach (IRailroad road in city.Outgoing)
                {
                    row[road.Destination.Name] = road.Length;
                }

                this.cityData.Add(row);
            }
        }

        /// <summary>
        /// Gets the index of the city in the data.
        /// </summary>
        /// <param name="cityName">Name of the city.</param>
        /// <returns>The index of the row corresponding to the city</returns>
        protected virtual int GetCityIndex(string cityName)
        {
            CityRow city = this.cityData.FirstOrDefault(c => c.Name == cityName);

            if (city == default(CityRow))
            {
                return -1;
            }

            return this.cityData.IndexOf(city);
        }

        /// <summary>
        /// Calculates the paths.
        /// </summary>
        private void CalculatePaths()
        {
            for (int k = 0; k < this.cityData.Count; k++)
            {
                for (int i = 0; i < this.cityData.Count; i++)
                {
                    for (int j = 0; j < this.cityData.Count; j++)
                    {
                        this.cityData[i][j] = Math.Min(this.cityData[i][j], this.cityData[i][k] + this.cityData[k][j]);
                    }
                }
            }
        }
    }
}
