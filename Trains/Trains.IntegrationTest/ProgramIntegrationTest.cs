using NUnit.Framework;
using System.Collections.Generic;
using System.Linq;
using Trains.Map;
using Trains.Plan;

namespace Trains.IntegrationTest
{
    [TestFixture]
    public class ProgramIntegrationTest
    {
        /// <summary>
        /// Represents a result for which there is no route.
        /// </summary>
        private const int NONE = -1;

        /// <summary>
        /// Represents an unreachable node
        /// </summary>
        private const int UNREACHABLE = NONE;

        /// <summary>
        /// Sample test data provided in the problem description.
        /// </summary>
        private const string SAMPLE_TEST_GRAPH = "AB5, BC4, CD8, DC8, DE6, AD5, CE2, EB3, AE7";

        /// <summary>
        /// Tests if use case #10 runs correctly
        /// </summary>
        /// <param name="railroadGraph">The railroad graph.</param>
        /// <param name="expectedCount">The expected count.</param>
        [TestCase("AC1, CA8, AB1, BC8", 6)]
        [TestCase("AC1, CA8", 3)]
        [TestCase("AC1, CA9", 2)]
        [TestCase("AC1, CA13", 2)]
        [TestCase("AC1, CA14", 1)]
        [TestCase("AC1, CA28", 1)]
        [TestCase(SAMPLE_TEST_GRAPH, 7)]
        [Test]
        public void TestIfUseCaseNumberTenRunsCorrectly(string railroadGraph, int expectedCount)
        {
            RailroadMap map = new RailroadMap();
            map.BuildMap(railroadGraph);
            IRouteFinder routeFinder = new RouteFinder(map);

            IEnumerable<int> counts = Trains.Program.RunCompoundSpecificationCountRoutesUseCase(routeFinder);

            Assert.AreEqual(expectedCount, counts.ElementAt(0));
        }

        /// <summary>
        /// Tests if use case one trough five run correctly
        /// </summary>
        /// <param name="railroadGraph">The railroad graph.</param>
        /// <param name="expectedResults">The expected results.</param>
        [TestCase("AB1, BC1, AD1, DC1, AE1, EB1, CD1, ED1", 2, 1, 2, 4, 2)]
        [TestCase("AB1, BC1, CD1, AE1, EB1", 2, NONE, NONE, 4, NONE)]
        [TestCase("AB1, BA1", NONE, NONE, NONE, NONE, NONE)]
        [TestCase("AB1", NONE, NONE, NONE, NONE, NONE)]
        [TestCase(SAMPLE_TEST_GRAPH, 9, 5, 13, 22, NONE)]
        [Test]
        public void TestIfUseCaseOneTroughFiveRunCorrectly(string railroadGraph, params int[] expectedResults)
        {
            RailroadMap map = new RailroadMap();
            map.BuildMap(railroadGraph);
            IRouteFinder routeFinder = new RouteFinder(map);

            IEnumerable<int> actualResults = Trains.Program.RunPathSpecificationUseCases(routeFinder);

            CollectionAssert.AreEquivalent(expectedResults, actualResults);
        }

        /// <summary>
        /// Tests if use cases six and seven run correctly
        /// </summary>
        /// <param name="railroadGraph">The railroad graph.</param>
        /// <param name="expectedResults">The expected results.</param>
        [TestCase("CB1, BD1, DC1, BC1", 2, 0)]
        [TestCase("AB1, BC1, CE1, EF1, EC1", 1, 1)]
        [TestCase("CB1, BD1, DC1", 1, 0)]
        [TestCase("AC1, CA1", 0, 1)]
        [TestCase("AB1", 0, 0)]
        [TestCase(SAMPLE_TEST_GRAPH, 2, 3)]
        [Test]
        public void TestIfUseCasesSixAndSevenRunCorrectly(string railroadGraph, params int[] expectedResults)
        {
            RailroadMap map = new RailroadMap();
            map.BuildMap(railroadGraph);
            IRouteFinder routeFinder = new RouteFinder(map);

            IEnumerable<int> actualResults = Trains.Program.RunRouteCountUseCases(routeFinder);

            CollectionAssert.AreEquivalent(expectedResults, actualResults);
        }

        /// <summary>
        /// Tests if use cases eight and nine run correctly
        /// </summary>
        /// <param name="railroadGraph">The railroad graph.</param>
        /// <param name="expectedResults">The expected results.</param>
        [TestCase("BC1, CB3, CA1, AB1", 2, 3)]
        [TestCase("AC1, BC1, CB1", 1, 2)]
        [TestCase("AB1, BC1", 2, UNREACHABLE)]
        [TestCase("BC1, CB1", UNREACHABLE, 2)]
        [TestCase("AB1, BC2, AC4", 3, UNREACHABLE)]
        [TestCase("AC1", 1, UNREACHABLE)]
        [TestCase("AB1, BD1", UNREACHABLE, UNREACHABLE)]
        [TestCase("AB1", UNREACHABLE, UNREACHABLE)]
        [TestCase(SAMPLE_TEST_GRAPH, 9, 9)]
        [Test]
        public void TestIfUseCasesEightAndNineRunCorrectly(string railroadGraph, params int[] expectedResults)
        {
            RailroadMap map = new RailroadMap();
            map.BuildMap(railroadGraph);

            IEnumerable<int> actualResults = Trains.Program.RunShortestRouteUseCases(map);

            CollectionAssert.AreEquivalent(expectedResults, actualResults);
        }
    }
}
