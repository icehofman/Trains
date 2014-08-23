using NSubstitute;
using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using Trains.Map;
using Trains.Plan;

namespace Trains.Test.Plan
{
    [TestFixture]
    public class ShortestLengthFinderTest
    {
        /// <summary>
        /// Tests if it can get the shortest length
        /// </summary>
        /// <param name="mapGraph">The map graph.</param>
        /// <param name="origin">The origin.</param>
        /// <param name="destination">The destination.</param>
        /// <param name="expectedDistance">The expected distance.</param>
        [TestCase("AB1", "A", "B", 1)] // Simplest possible test case    
        [TestCase("AB1 BA2", "B", "A", 2)]
        [TestCase("AB1 BA1", "B", "B", 2)] // Simplest possible test case that has a cycle and starts at second railroad
        [TestCase("AB1 BC1", "A", "C", 2)]
        [TestCase("AB1 BC1 CD1 AD1", "A", "D", 1)]
        [TestCase("AB1 BC2 CD1 AC1 BD1", "A", "D", 2)]
        [TestCase("AB5 BC4 CD8 DC8 DE6 AD5 CE2 EB3 AE7", "A", "C", 9)]
        [TestCase("AB1 BC1 CA1", "B", "B", 3)]
        [TestCase("AB1 BC1 CA1", "A", "A", 3)]
        [TestCase("AB1 AC2 BD3 AD1 DA1", "B", "A", 4)]
        [TestCase("BC4 CD8 DC8 CE2 EB3", "B", "B", 9)]
        [TestCase("AB5 BC4 CD8 DC8 DE6 AD5 CE2 EB3 AE7", "B", "B", 9)]
        [TestCase("AB1", "B", "A", ShortestLengthFinder.Unreachable)]
        [TestCase("AB1", "A", "C", ShortestLengthFinder.Unreachable)]
        public void TestIfItCanGetTheShortestLength(string mapGraph, string origin, string destination, int expectedDistance)
        {
            // Arrange
            // Arrange the map
            IRailroadMap map = Substitute.For<IRailroadMap>();
            IList<ICity> cities = TestHelper.GenerateCities(mapGraph, true).ToArray();
            map.Cities.Returns(cities);

            // Arrange the target
            ShortestLengthFinder target = new ShortestLengthFinder(map);

            // Act
            int actualLengthFound = target.GetShortestLength(from: origin, to: destination);

            // Assert
            Assert.That(actualLengthFound, Is.EqualTo(expectedDistance));
        }

        /// <summary>
        /// Tests if it can initialize correctly
        /// </summary>
        /// <param name="mapGraph">The map graph.</param>
        /// <param name="originIndex">Index of the origin.</param>
        /// <param name="destinationsLength">Length of the destinations.</param>
        [TestCase("AB1", 0, ShortestLengthFinder.Unreachable, 1)]
        [TestCase("AB1", 1, ShortestLengthFinder.Unreachable, ShortestLengthFinder.Unreachable)]
        [TestCase("AB1 AC2", 0, ShortestLengthFinder.Unreachable, 1, 2)]
        [TestCase("AB1 BA2", 1, 2, ShortestLengthFinder.Unreachable)]
        [TestCase("AB1 AC2 BC3", 0, ShortestLengthFinder.Unreachable, 1, 2)]
        [TestCase("AB1 AC2 BC3", 1, ShortestLengthFinder.Unreachable, ShortestLengthFinder.Unreachable, 3)]
        public void TestIfItCanInitializeCorrectly(string mapGraph, int originIndex, params int[] destinationsLength)
        {
            // Arrange
            IRailroadMap map = Substitute.For<IRailroadMap>();
            IList<ICity> cities = TestHelper.GenerateCities(mapGraph, true).ToArray();
            map.Cities.Returns(cities);

            // Act
            var target = new Trains.Plan.ShortestLengthFinder(map);
            var rows = target.GetPrivateField<IList<ShortestLengthFinder.CityRow>>("cityData");

            // Assert
            ShortestLengthFinder.CityRow row = rows.ElementAt(originIndex);
            for (int i = 0; i < destinationsLength.Length; i++)
            {
                Assert.That(row[i], Is.EqualTo(destinationsLength[i]), "Unequality at: {0}", i);
            }
        }

        /// <summary>
        /// Tests if city row can store and retrieve data
        /// </summary>
        /// <param name="neighbors">The neighbors.</param>
        /// <param name="key">The key.</param>
        /// <param name="expectedData">The expected data.</param>
        [TestCase(new[] { "A", "B", "C" }, 0, 1)]
        [TestCase(new[] { "A", "B", "C" }, "B", 1)]
        public void TestIfCityRowCanStoreAndRetrieveData(string[] neighbors, object key, int expectedData)
        {
            // Arrange
            var target = new ShortestLengthFinder.CityRow("A", neighbors);
            int actualData = int.MinValue;

            // Act
            string getKey = key as string;
            if (getKey != null)
            {
                target[getKey] = expectedData;
                actualData = target[getKey];
            }
            else
            {
                int indexKey = (int)key;
                target[indexKey] = expectedData;
                actualData = target[indexKey];
            }

            // Assert
            Assert.That(actualData, Is.EqualTo(expectedData));
        }
    }
}
