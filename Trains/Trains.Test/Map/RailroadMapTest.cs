using NUnit.Framework;
using System.IO;
using System.Linq;
using Trains.Map;

namespace Trains.Test.Map
{
    [TestFixture]
    public class RailroadMapTest
    {
        /// <summary>
        /// The file path of the single rail road test data
        /// </summary>
        private const string SINGLE_RAILROAD_DATA = "test_data/single_railroad_two_cities.txt";

        /// <summary>
        /// Test data for the build railroads testcase
        /// </summary>        
        private static object[] buildRailroadsTestData =
        {
            new object[] { "AB1", new string[] { "A", "B" }, new int[] { 1 } },
            new object[] { "AB1, CD2", new string[] { "A", "B", "C", "D" }, new int[] { 1, 2 } },
            new object[] { "AB1, BA1", new string[] { "A", "B" }, new int[] { 1, 1 } },
            new object[] { "AB1, CD2, BA2", new string[] { "A", "B", "C", "D" }, new int[] { 1, 2, 2 } },
            new object[] { "AB5, BC4, CD8, DC8, DE6, AD5, CE2, EB3, AE7", new string[] { "A", "B", "C", "D", "E" }, new int[] { 5, 4, 8, 8, 6, 5, 2, 3, 7 } }
        };

        /// <summary>
        /// Tests if it doesnt break with A valid file
        /// </summary>
        [Test]
        public void TestIfItDoesNotBreakWithAValidFile()
        {
            // Arrange            
            RailroadMap target;

            // Act
            target = new RailroadMap();
            target.Init(File.OpenRead(SINGLE_RAILROAD_DATA));

            // Assert
            Assert.Pass();
        }

        /// <summary>
        /// Tests if it can build A single railroad with a graph string AB1
        /// </summary>
        [Test]
        public void TestIfItCanBuildASingleRailroad()
        {
            // Arrange
            RailroadMap target;
            string graphString = "AB1";

            // Act
            target = new RailroadMap();
            target.BuildMap(graphString);

            // Assert
            Assert.AreEqual(1, target.Railroads.Count());
            Assert.AreEqual(2, target.Cities.Count());
            IRailroad railroad = target.Railroads.ElementAt(0);
            Assert.AreEqual(1, railroad.Length);
            var cityA = target.Cities.ElementAt(0);
            Assert.AreEqual("A", cityA.Name);
            var cityB = target.Cities.ElementAt(1);
            Assert.AreEqual("B", cityB.Name);
            Assert.AreEqual(railroad.Origin, cityA);
            Assert.AreEqual(railroad.Destination, cityB);
        }

        /// <summary>
        /// Tests if it can build railroads
        /// </summary>
        /// <param name="graphPath">The graph path.</param>
        /// <param name="orderedCityNames">The ordered city names.</param>
        /// <param name="railroadLengths">The railroad lengths.</param>
        [Test]
        [TestCaseSource("buildRailroadsTestData")]
        public void TestIfItCanBuildRailroads(string graphPath, string[] orderedCityNames, int[] railroadLengths)
        {
            // Arrange
            var map = new RailroadMap();
            int expectedCityCount = orderedCityNames.Length;
            int expectedRailroadCount = railroadLengths.Length;

            // Act
            map.BuildMap(graphPath);

            // Assert
            Assert.AreEqual(expectedCityCount, map.Cities.Count());
            Assert.AreEqual(expectedRailroadCount, map.Railroads.Count());
            CollectionAssert.AreEquivalent(orderedCityNames, map.Cities.Select(city => city.Name));
            CollectionAssert.AreEquivalent(railroadLengths, map.Railroads.Select(rr => rr.Length));
        }

        /// <summary>
        /// Tests if it can read file content
        /// </summary>
        [Test]
        public void TestIfItCanReadFileContent()
        {
            // Arrange
            var method = TestHelper.GetPrivateStaticMethod<RailroadMap>("ReadContent");
            string content = string.Empty;
            string expected = "AB1";

            // Act
            content = (string)method.Invoke(File.OpenRead(SINGLE_RAILROAD_DATA));

            // Assert
            Assert.AreEqual(expected, content);
        }

        /// <summary>
        /// Tests if it can create city
        /// </summary>
        [Test]
        public void TestIfItCanCreateCity()
        {
            // Arrange
            RailroadMap target = new RailroadMap();
            var method = target.GetPrivateMethod("GetOrCreateCity");
            string cityName = "Z";

            // Act            
            method.Invoke(cityName);

            // Assert
            Assert.IsTrue(target.Cities.Any(city => city.Name == cityName));
        }
    }
}
