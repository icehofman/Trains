﻿using NSubstitute;
using NUnit.Framework;
using Trains.Map;
using Trains.Plan;
using Trains.Specify;

namespace Trains.Test.Specify
{
    [TestFixture]
    public class OriginAndEndSpecificationTest
    {
        /// <summary>
        /// It contains the test data used for configuring the routes for this PathSpecificationTest
        /// </summary>
        private static readonly object[] TestDataForSatisfiedBy = { new object[] { "A", "B", "A", "B", true }, new object[]  { "A", "C", "A", "B", false }, new object[] { "A", "A", "A", "A", true }, new object[] { "A", "A", "B", "A", false } };

        /// <summary>
        /// It contains the test data used for configuring the routes for this PathSpecificationTest
        /// </summary>
        private static readonly object[] TestDataForMightBeSatisfiedBy = { new object[] { "A", "B", "A", "B", true }, new object[] { "A", "C", "A", "B", true }, new object[] { "A", "A", "A", "A", true }, new object[] { "A", "A", "B", "A", false } };

        /// <summary>
        /// Tests if it can specify origin and destination correctly
        /// </summary>
        /// <param name="routeOrigin">The route origin.</param>
        /// <param name="routeDestination">The route destination.</param>
        /// <param name="origin">The origin.</param>
        /// <param name="destination">The destination.</param>
        /// <param name="expectedResult">if set to <c>true</c> [expected result].</param>
        [Test]
        [TestCaseSource("TestDataForSatisfiedBy")]
        public void TestIfItCanSpecifyOriginAndDestinationCorrectly(string routeOrigin, string routeDestination, string origin, string destination, bool expectedResult)
        {
            var target = new OriginAndDestinationSpecification(origin, destination);
            IRoute route = Substitute.For<IRoute>();
            var originCity = Substitute.For<ICity>();
            var destinationCity = Substitute.For<ICity>();
            originCity.Name = routeOrigin;
            destinationCity.Name = routeDestination;
            route.Origin.Returns(originCity);
            route.Destination.Returns(destinationCity);

            bool actual = target.IsSatisfiedBy(route);

            Assert.AreEqual(expectedResult, actual);
            Assert.Null(route.Received().Destination);
            if (destination == routeDestination)
            {
                Assert.Null(route.Received().Origin);
            }
        }

        /// <summary>
        /// Tests if it knows when a route might satisfy
        /// </summary>
        /// <param name="routeOrigin">The route origin.</param>
        /// <param name="routeDestination">The route destination.</param>
        /// <param name="origin">The origin.</param>
        /// <param name="destination">The destination.</param>
        /// <param name="expectedResult">if set to <c>true</c> [expected result].</param>
        [Test]
        [TestCaseSource("TestDataForMightBeSatisfiedBy")]
        public void TestIfItKnowsWhenARouteMightSatisfy(string routeOrigin, string routeDestination, string origin, string destination, bool expectedResult)
        {
            var target = new OriginAndDestinationSpecification(origin, destination);
            IRoute route = Substitute.For<IRoute>();
            var originCity = Substitute.For<ICity>();
            var destinationCity = Substitute.For<ICity>();
            originCity.Name = routeOrigin;
            destinationCity.Name = routeDestination;
            route.Origin.Returns(originCity);
            route.Destination.Returns(destinationCity);

            bool actual = target.MightBeSatisfiedBy(route);

            Assert.AreEqual(expectedResult, actual);
            Assert.Null(route.Received().Origin);
        }
    }
}
