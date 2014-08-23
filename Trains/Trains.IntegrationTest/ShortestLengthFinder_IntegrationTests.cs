using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trains.Map;
using Trains.Plan;

namespace Trains.IntegrationTest
{
    [TestFixture]
    public class ShortestLengthFinder_IntegrationTests
    {
        /// <summary>
        /// Tests if it can get the shortest length given a map
        /// </summary>
        /// <param name="mapGraph">The map graph.</param>
        /// <param name="origin">The origin.</param>
        /// <param name="destination">The destination.</param>
        /// <param name="expectedLength">The expected length.</param>
        [TestCase("AB5, BC4, CD8, DC8, DE6, AD5, CE2, EB3, AE7", "A", "C", 9)]

        // Use case #9
        [TestCase("AB5, BC4, CD8, DC8, DE6, AD5, CE2, EB3, AE7", "B", "B", 9)]

        // Fully connected triangle
        [TestCase("AB1, BA4, BC2, CB3, AC3, CA2", "C", "C", 5)]
        [TestCase("AB1, BC2, CB3, AC3", "C", "A", ShortestLengthFinder.Unreachable)]

        // Fully connected square
        [TestCase("AB1, BA4, BC2, CB3, CD3, DC2, DA4, AD1, AC3, CA2, BD1, DB4", "B", "B", 5)]

        // Partially connected square (1 route only for answer)
        [TestCase("BA4, CB3, CD3, DA4, AD1, CA2, AC3, BD1", "B", "B", 10)]
        public void TestIfItCanGetTheShortestLengthGivenAMap(string mapGraph, string origin, string destination, int expectedLength)
        {
            // Arrange
            RailroadMap map = new RailroadMap();
            map.BuildMap(mapGraph);
            var target = new ShortestLengthFinder(map);

            // Act
            int actualResult = target.GetShortestLength(from: origin, to: destination);

            // Assert
            Assert.That(actualResult, Is.EqualTo(expectedLength));
        }
    }
}
