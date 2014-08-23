using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Trains.Map;
using Trains.Plan;
using Trains.Specify;

namespace Trains.Test.Specify
{
    [TestFixture]
    public class StopsCountSpecificationTest
    {
        /// <summary>
        /// Test data used to verify it can count stops correctly
        /// </summary>
        private static object[] testData = 
        {
            // Minimum stop count, maximum stop count, actual stop count, expected result
            // Test cases when minimum and maximum are the same            
            new object[] { 1, 1, 1, true },
            new object[] { 10, 10, 10, true },
            new object[] { 2, 2, 1, false },
            new object[] { 2, 2, 3, false },
            new object[] { 0, 0, 1, false },
            new object[] { 1, 1, 0, false },

            // Test cases when minumum and maximum are different
            // Within range
            new object[] { 0, 1, 1, true },
            new object[] { 0, 1, 0, true },
            new object[] { 1, 2, 2, true },

            // Outside of the range
            new object[] { 0, 1, 2, false },
            new object[] { 1, 2, 0, false },
            new object[] { 2, 3, 1, false }, // Too low
            new object[] { 2, 3, 4, false }, // Too high
        };

        /// <summary>
        /// Test data used to verify it can determine if a route might satisfy in the future (or not)
        /// </summary>
        private static object[] testDataForMightSatisfy = 
        {
            // Minimum stop count, maximum stop count, actual stop count, expected result
            // NOTE: Currently satisfying routes also return tru for "Might Satisfy"
            // Test cases when minimum and maximum are the same            
            new object[] { 1, 1, 1, true },
            new object[] { 10, 10, 10, true },
            new object[] { 2, 2, 1, true },
            new object[] { 1, 1, 0, true },
            new object[] { 1, 2, 0, true },

            // Test cases when minumum and maximum are different
            // Within range
            new object[] { 0, 1, 1, true },
            new object[] { 0, 1, 0, true },
            new object[] { 1, 2, 2, true },
            new object[] { 2, 3, 1, true },

            // Outside of the range
            new object[] { 0, 0, 1, false },
            new object[] { 2, 2, 3, false },
            new object[] { 0, 1, 2, false },
            new object[] { 2, 3, 4, false },
        };

        /// <summary>
        /// Tests if stops count specification can validate correctly
        /// </summary>
        /// <param name="minimumStopCount">The route stop count.</param>
        /// <param name="maximumStopCount">The maximum stop count.</param>
        /// <param name="actualStopCount">The actual stop count.</param>
        /// <param name="expectedResult">if set to <c>true</c> [expected result].</param>
        [Test]
        [TestCaseSource("testData")]
        public void TestIfItKnowsWhenARouteSatisfies(int minimumStopCount, int maximumStopCount, int actualStopCount, bool expectedResult)
        {
            // Arrange
            var target = new StopsCountSpecification(minimumStopCount, maximumStopCount);
            var route = Substitute.For<IRoute>();
            var legs = new List<IRailroad>();
            for (int i = 0; i < actualStopCount; i++)
            {
                legs.Add(Substitute.For<IRailroad>());
            }

            route.Legs.Returns(legs);

            // Act
            bool actual = target.IsSatisfiedBy(route);

            // Assert
            Assert.AreEqual(expectedResult, actual);
        }

        /// <summary>
        /// Tests if it knows when A route might satisfy
        /// </summary>
        /// <param name="minimumStopCount">The minimum stop count.</param>
        /// <param name="maximumStopCount">The maximum stop count.</param>
        /// <param name="specStopCount">The spec stop count.</param>
        /// <param name="expectedResult">if set to <c>true</c> [expected result].</param>
        [Test]
        [TestCaseSource("testDataForMightSatisfy")]
        public void TestIfItKnowsWhenARouteMightSatisfy(int minimumStopCount, int maximumStopCount, int specStopCount, bool expectedResult)
        {
            // Arrange
            var target = new StopsCountSpecification(minimumStopCount, maximumStopCount);
            var route = Substitute.For<IRoute>();
            var legs = new List<IRailroad>();
            for (int i = 0; i < specStopCount; i++)
            {
                legs.Add(Substitute.For<IRailroad>());
            }

            route.Legs.Returns(legs);

            // Act
            bool actual = target.MightBeSatisfiedBy(route);

            // Assert
            Assert.AreEqual(expectedResult, actual);
        }
    }
}
