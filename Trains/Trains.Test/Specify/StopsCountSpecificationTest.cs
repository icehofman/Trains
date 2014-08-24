using NSubstitute;
using NUnit.Framework;
using System.Collections.Generic;
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
        private static object[] testData = { new object[] { 1, 1, 1, true }, new object[] { 10, 10, 10, true }, new object[] { 2, 2, 1, false }, new object[] { 2, 2, 3, false }, new object[] { 0, 0, 1, false }, new object[] { 1, 1, 0, false },
            new object[] { 0, 1, 1, true }, new object[] { 0, 1, 0, true }, new object[] { 1, 2, 2, true }, new object[] { 0, 1, 2, false }, new object[] { 1, 2, 0, false }, new object[] { 2, 3, 1, false }, new object[] { 2, 3, 4, false }, };

        /// <summary>
        /// Test data used to verify it can determine if a route might satisfy in the future (or not)
        /// </summary>
        private static object[] testDataForMightSatisfy = { new object[] { 1, 1, 1, true }, new object[] { 10, 10, 10, true }, new object[] { 2, 2, 1, true }, new object[] { 1, 1, 0, true }, new object[] { 1, 2, 0, true },
            new object[] { 0, 1, 1, true }, new object[] { 0, 1, 0, true }, new object[] { 1, 2, 2, true }, new object[] { 2, 3, 1, true }, new object[] { 0, 0, 1, false }, new object[] { 2, 2, 3, false }, new object[] { 0, 1, 2, false }, new object[] { 2, 3, 4, false }, };

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
            var target = new StopsCountSpecification(minimumStopCount, maximumStopCount);
            var route = Substitute.For<IRoute>();
            var legs = new List<IRailroad>();
            for (int i = 0; i < actualStopCount; i++)
            {
                legs.Add(Substitute.For<IRailroad>());
            }

            route.Legs.Returns(legs);

            bool actual = target.IsSatisfiedBy(route);

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
            var target = new StopsCountSpecification(minimumStopCount, maximumStopCount);
            var route = Substitute.For<IRoute>();
            var legs = new List<IRailroad>();
            for (int i = 0; i < specStopCount; i++)
            {
                legs.Add(Substitute.For<IRailroad>());
            }

            route.Legs.Returns(legs);

            bool actual = target.MightBeSatisfiedBy(route);

            Assert.AreEqual(expectedResult, actual);
        }
    }
}
