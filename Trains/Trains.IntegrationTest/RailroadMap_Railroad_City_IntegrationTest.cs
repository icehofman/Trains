using NUnit.Framework;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Trains.Map;

namespace Trains.IntegrationTest
{
    [TestFixture]
    public class RailroadMap_Railroad_City_IntegrationTest
    {
        /// <summary>
        /// Tests that the RailroadMap can generate a single rail road.
        /// </summary>
        [Test]
        public void TestCanGenerateASingleRailroad()
        {
            // Arrange
            RailroadMap map;
            IEnumerable<IRailroad> railroads;
            IRailroad railroad;
            IEnumerable<ICity> cities;
            ICity originCity;
            ICity destinationCity;
            int expectedLength = 1;
            int expectedRailRoadCount = 1;
            int expectedCityCount = 2;
            string expectedOriginCityName = "A";
            string expectedDesintationCityName = "B";
            string filePath = "test_data/single_railroad_two_cities.txt";
            var testDataFileStream = File.OpenRead(filePath);

            // Act
            map = new RailroadMap();
            map.Init(testDataFileStream);
            railroads = map.Railroads;
            cities = map.Cities;

            // Assert
            // Verify that collections are obtained correctly.
            Assert.AreEqual(expectedRailRoadCount, railroads.Count());
            Assert.AreEqual(expectedCityCount, cities.Count());

            // Verify that cities and railroads are read correctly
            railroad = railroads.ElementAt(0);
            originCity = cities.First(city => city.Name == expectedOriginCityName);
            destinationCity = cities.First(city => city.Name == expectedDesintationCityName);
            Assert.AreEqual(expectedLength, railroad.Length);

            // Verify cities and railroads are connected correctly
            Assert.AreEqual(originCity, railroad.Origin);
            Assert.AreEqual(destinationCity, railroad.Destination);
            Assert.AreEqual(originCity.Outgoing.ElementAt(0), railroad);
        }
    }
}
