using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Trains.Map
{
    public class RailroadMap : IRailroadMap
    {
        /// <summary>
        /// The railroads in the map
        /// </summary>
        private IList<IRailroad> railroads;

        /// <summary>
        /// The cities in the map
        /// </summary>
        private IList<ICity> cities;

        /// <summary>
        /// Initializes a new instance of the <see cref="RailroadMap"/> class.
        /// </summary>
        public RailroadMap()
        {
            this.railroads = new List<IRailroad>();
            this.cities = new List<ICity>();
        }

        #region Properties
        /// <summary>
        /// Gets the rail roads read from the file stream.
        /// </summary>        
        public IEnumerable<IRailroad> Railroads
        {
            get
            {
                return this.railroads;
            }
        }

        /// <summary>
        /// Gets the cities read from the file stream.
        /// </summary>
        public IEnumerable<ICity> Cities
        {
            get
            {
                return this.cities;
            }
        }
        #endregion
        #region Public methods
        /// <summary>
        /// Initializes this class with the railroads and cities.
        /// </summary>
        /// <param name="stream">The stream of the configuration file.</param>
        public virtual void Init(FileStream stream)
        {
            string configGraph = string.Empty;
            configGraph = RailroadMap.ReadContent(stream);
            this.BuildMap(configGraph);
        }

        /// <summary>
        /// Builds the railroad map.
        /// Expected graph format examples: [AB1,BC1] or [AB1, BC1]
        /// </summary>
        /// <param name="graph">The string with the railroads configuration graph.</param>
        public virtual void BuildMap(string graph)
        {
            string[] paths = graph.Split(new string[] { ", ", "," }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string path in paths)
            {
                string originCityName = path.Substring(0, 1);
                string destinationCityName = path.Substring(1, 1);
                int railroadLength = int.Parse(path.Substring(2));

                ICity originCity = this.GetOrCreateCity(originCityName);
                ICity destinationCity = this.GetOrCreateCity(destinationCityName);
                IRailroad newRailroad = new Railroad();
                newRailroad.Origin = originCity;
                newRailroad.Destination = destinationCity;
                newRailroad.Length = railroadLength;
                this.railroads.Add(newRailroad);
                originCity.Outgoing.Add(newRailroad);
            }
        }
        #endregion
        #region Private methods
        /// <summary>
        /// Reads the file stream content.
        /// </summary>
        /// <param name="stream">The file stream.</param>
        /// <returns>The configuration of the paths in the map</returns>
        private static string ReadContent(FileStream stream)
        {
            string configPath;
            using (var reader = new StreamReader(stream))
            {
                configPath = reader.ReadLine();
            }

            return configPath.Replace("Graph: ", string.Empty);
        }

        /// <summary>
        /// Gets or creates a city.
        /// </summary>
        /// <param name="originCityName">Name of the origin city.</param>
        /// <returns>The city with the name <paramref name="originCityName"/></returns>
        private ICity GetOrCreateCity(string originCityName)
        {
            ICity city = this.cities.FirstOrDefault(item => item.Name == originCityName);
            if (city == default(ICity))
            {
                city = new City();
                city.Name = originCityName;
                this.cities.Add(city);
            }

            return city;
        }
        #endregion
    }
}
