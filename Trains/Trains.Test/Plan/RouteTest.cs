using NSubstitute;
using NUnit.Framework;
using System.Linq;
using Trains.Map;
using Trains.Plan;

namespace Trains.Test.Plan
{
    [TestFixture]
    public class RouteTest
    {
        /// <summary>
        /// Test data for the distance validation test
        /// </summary>
        private static object[] distanceData = { new object[] { new int[] { 1 } }, new object[] { new int[] { 0 } }, new object[] { new int[] { 2 } }, new object[] { new int[] { 1, 0 } }, new object[] { new int[] { 1, 1 } }, new object[] { new int[] { 1, 1, 1 } } };

        /// <summary>
        /// Test data for the origin and destination test
        /// </summary>
        private static object[] cityData ={ new object[] { new string[] { "A", "B" } }, new object[] { new string[] { "A", "B", "C" } }, new object[] { new string[] { "A", "B", "A" } }, new object[] { new string[] { "A", "B", "A", "C" } } };

        /// <summary>
        /// Tests if it can calculate distance correctly
        /// </summary>
        /// <param name="legDistances">The leg distances.</param>
        [Test]
        [TestCaseSource("distanceData")]
        public void TestIfItCanCalculateDistanceCorrectly(int[] legDistances)
        {
            var target = new Route();
            for (int i = 0; i < legDistances.Length; i++)
            {
                IRailroad mockRailRoad = Substitute.For<IRailroad>();
                mockRailRoad.Length = legDistances[i];
                target.AddLeg(mockRailRoad);
            }

            int expectedTotalDistance = legDistances.Sum();

            int actualTotalDistance = target.Distance;

            Assert.AreEqual(expectedTotalDistance, actualTotalDistance);
        }

        /// <summary>
        /// Tests if it know its origin and destination
        /// </summary>
        /// <param name="cityNames">The city names.</param>
        [Test]
        [TestCaseSource("cityData")]
        public void TestIfItKnowItsOriginAndDestination(params string[] cityNames)
        {
            var target = new Route();
            string expectedOrigin = cityNames[0];
            string expectedDestination = cityNames[cityNames.Length - 1];

            for (int i = 0; i < cityNames.Length - 1; i++)
            {
                var originCity = Substitute.For<ICity>();
                originCity.Name = cityNames[i];
                var destinationCity = Substitute.For<ICity>();
                destinationCity.Name = cityNames[i + 1];
                IRailroad leg = Substitute.For<IRailroad>();
                leg.Origin = originCity;
                leg.Destination = destinationCity;
                target.AddLeg(leg);
            }

            Assert.AreEqual(expectedOrigin, target.Origin.Name);
            Assert.AreEqual(expectedDestination, target.Destination.Name);
        }

        /// <summary>
        /// Tests if it transforms to string correctly
        /// </summary>
        /// <param name="routePath">The route path.</param>
        [TestCase("AB1 BC1 CD1")]

        [TestCase("AB1 BC1")]

        [TestCase("AB1")]

        [TestCase("")]

        [Test]
        public void TestIfItTransformsToStringCorrectly(string routePath)
        {
            Route route = TestHelper.BuildRouteFromString(routePath);
            string expectedString = string.IsNullOrEmpty(routePath) ? "{ }" : "{ " + routePath + " }";

            string actualString = route.ToString();

            Assert.AreEqual(expectedString, actualString);
        }

        /// <summary>
        /// Tests if it knows when another instance is equal
        /// </summary>
        /// <param name="routeAGraph">The route A graph.</param>
        /// <param name="routeBGraph">The route B graph.</param>
        /// <param name="expectedResult">if set to <c>true</c> [expected result].</param>
        [TestCase("AB1 BC1", "AB1 BD1", false)]

        [TestCase("AB1 BC2", "AB1 BC1", false)]

        [TestCase("AB1", "AB1 BC1", false)]

        [TestCase("AB2", "AB1", false)]

        [TestCase("AC1", "AB1", false)]

        [TestCase("AB1 BC1", "AB1 BC1", true)]

        [TestCase("AB1", "AB1", true)]

        [Test]
        public void TestIfItKnowsWhenAnotherInstanceIsEqual(string routeAGraph, string routeBGraph, bool expectedResult)
        {
            Route routeA = TestHelper.BuildRouteFromString(routeAGraph);
            Route routeB = TestHelper.BuildRouteFromString(routeBGraph);

            bool actualResult = routeA.Equals(routeB);

            Assert.AreEqual(expectedResult, actualResult);
            if (expectedResult)
            {
                Assert.AreEqual(routeA.GetHashCode(), routeB.GetHashCode());
            }
            else
            {
                Assert.AreNotEqual(routeA.GetHashCode(), routeB.GetHashCode());
            }
        }
    }
}
