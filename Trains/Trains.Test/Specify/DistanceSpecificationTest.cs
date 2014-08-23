using NSubstitute;
using NUnit.Framework;
using Trains.Plan;
using Trains.Specify;

namespace Trains.Test.Specify
{
    [TestFixture]
    public class DistanceSpecificationTest
    {
        /// <summary>
        /// Test data to specify route distance, min and max specified distance, and expected statisfaction result.
        /// </summary>
        private static object[] testDataForSatisfiedTest =
        {
            // route distance, min distance, max distance, expected result
            // Min and max distances are the same
            // Valid distances
            new object[] { 0, 0, 0, true },
            new object[] { 1, 1, 1, true },
            new object[] { 2, 2, 2, true },

            // Invalid distances
            new object[] { 1, 0, 0, false },
            new object[] { 0, 1, 1, false },
            new object[] { 2, 1, 1, false },
            new object[] { 1, 2, 2, false },
            new object[] { 3, 2, 2, false },

            // Min and max distances are different
            // Valid distances
            new object[] { 0, 0, 1, true },
            new object[] { 1, 0, 1, true },
            new object[] { 0, 0, 2, true },
            new object[] { 1, 0, 2, true },
            new object[] { 2, 0, 2, true },
            new object[] { 1, 1, 2, true },
            new object[] { 2, 1, 2, true },
            new object[] { 1, 1, 3, true },
            new object[] { 2, 1, 3, true },
            new object[] { 3, 1, 3, true },

            // Invalid distances
            new object[] { 2, 0, 1, false },            
            new object[] { 3, 0, 2, false },
            new object[] { 0, 1, 2, false },
            new object[] { 3, 1, 2, false },
            new object[] { 1, 2, 4, false },
        };

        /// <summary>
        /// Test data for the <typeparamref name="RouteCalculator.Plan.DistanceSpecification.MightBeSatisfiedBy"/> unit tests
        /// </summary>
        private static object[] testDataForMightBeSatisfiedTest =
        {
            // Valid distances
            new object[] { 0, 0, 0, true },
            new object[] { 0, 0, 1, true },
            new object[] { 1, 0, 1, true },
            new object[] { 0, 1, 1, true },
            new object[] { 1, 1, 1, true },
            new object[] { 1, 1, 2, true },
            new object[] { 2, 1, 2, true },
            new object[] { 0, 1, 2, true },
            new object[] { 1, 0, 2, true },

            // Invalid distances
            new object[] { 1, 0, 0, false },
            new object[] { 3, 2, 2, false },
            new object[] { 2, 0, 1, false },
            new object[] { 3, 1, 2, false },
            new object[] { 4, 1, 3, false },
        };

        /// <summary>
        /// Tests if it can specify correctly the required distance
        /// </summary>
        /// <param name="routeDistance">The route distance.</param>
        /// <param name="minDistance">The min distance.</param>
        /// <param name="maxDistance">The max distance.</param>
        /// <param name="expectedResult">if set to <c>true</c> [expected result].</param>
        [Test]
        [TestCaseSource("testDataForSatisfiedTest")]
        public void TestIfItCanSpecifyCorrectlyTheRequiredDistance(int routeDistance, int minDistance, int maxDistance, bool expectedResult)
        {
            // Arrange
            var target = new DistanceSpecification(minDistance, maxDistance);
            IRoute route = Substitute.For<IRoute>();
            route.Distance.Returns(routeDistance);

            // Act
            bool actualResult = target.IsSatisfiedBy(route);

            // Assert
            Assert.AreEqual(expectedResult, actualResult);
            Assert.AreEqual(0, route.Received().Distance);
        }

        /// <summary>
        /// Tests if it can correctly determine if A route might satisfy
        /// </summary>
        /// <param name="routeDistance">The route distance.</param>
        /// <param name="minDistance">The min distance.</param>
        /// <param name="maxDistance">The max distance.</param>
        /// <param name="expectedResult">if set to <c>true</c> [expected result].</param>
        [Test]
        [TestCaseSource("testDataForMightBeSatisfiedTest")]
        public void TestIfItCanCorrectlyDetermineIfARouteMightSatisfy(int routeDistance, int minDistance, int maxDistance, bool expectedResult)
        {
            // Arrange
            var target = new DistanceSpecification(minDistance, maxDistance);
            IRoute route = Substitute.For<IRoute>();
            route.Distance.Returns(routeDistance);

            // Act
            bool actualResult = target.MightBeSatisfiedBy(route);

            // Assert
            Assert.AreEqual(expectedResult, actualResult);
            Assert.AreEqual(0, route.Received().Distance);
        }
    }
}
