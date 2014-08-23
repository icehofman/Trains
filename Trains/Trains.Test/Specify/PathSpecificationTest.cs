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
    public class PathSpecificationTest
    {
        /// <summary>
        /// Test data used for the test of the MightBySatisfiedBy test
        /// </summary>
        private static object[] testDataForMightSatisfyTest =
        {
            // Valid
            // Satisfied
            new object[] 
            {
                new string[] { "AB" }, new string[] { "A", "B" }, true
            },
            new object[] 
            {
                new string[] { "AB", "BC" }, new string[] { "A", "B", "C" }, true
            },            

            // Not satisfied but still can
            // Simplest possible
            new object[] 
            {
                new string[] { "AB" }, new string[] { "A", "B", "C" }, true
            },
            new object[] 
            {
                new string[] { "AB", "BC" }, new string[] { "A", "B", "C", "D" }, true
            },

            // Invalid
            // Satisfied and exceded
            new object[] 
            {
                new string[] { "AB", "CD" }, new string[] { "A", "B" }, false
            },
            new object[] 
            {
                new string[] { "AB", "CD", "DE" }, new string[] { "A", "B" }, false
            },
            new object[] 
            {
                new string[] { "AB", "CD", "DE" }, new string[] { "A", "B", "C", "D" }, false
            },

            // Not satisfied and exceded
            new object[] 
            {
                new string[] { "AC", "CD" }, new string[] { "A", "B" }, false
            },
            new object[] 
            {
                new string[] { "AC", "CD", "DE" }, new string[] { "A", "C", "B" }, false
            },

            // Not satisfied and not exceded
            new object[] 
            {
                new string[] { "AC" }, new string[] { "A", "B" }, false
            },
            new object[] 
            {
                new string[] { "AC", "CD" }, new string[] { "A", "C", "B" }, false
            },
        };

        /// <summary>
        /// It contains the test data used for configuring the routes ofr this PathSpecificationTest
        /// </summary>
        private static object[] routesTestDataConfiguration =
        {
            new object[] 
            {
                new string[] { "AB" }, new string[] { "A", "B" }, true
            },
            new object[] 
            {
                new string[] { "AB", "CD", "BC" }, new string[] { "A", "B" }, false
            },
            new object[] 
            {
                new string[] { "AB", "BC", "CD" }, new string[] { "A", "B", "C", "D" }, true
            },
            new object[] 
            {
                new string[] { "AB", "BC", "CD" }, new string[] { "A", "B", "C", "E" }, false
            },
            new object[] 
            {
                new string[] { "AE", "EB", "BC", "CD" }, new string[] { "A", "E", "B", "C", "D" }, true
            }
        };

        /// <summary>
        /// Tests if trip specification can specify A cities route
        /// </summary>
        /// <param name="actualRoute">The actual route configuration.</param>
        /// <param name="specifiedRoute">The specified cities route.</param>
        /// <param name="expectedResult">if set to <c>true</c> [expected result].</param>
        [Test]
        [TestCaseSource("routesTestDataConfiguration")]
        public void TestIfTripSpecificationCanSpecifyACitiesRoute(string[] actualRoute, string[] specifiedRoute, bool expectedResult)
        {
            // Arrange
            PathSpecification pathSpec = new PathSpecification(specifiedRoute);
            IRoute route = Substitute.For<IRoute>();
            IList<IRailroad> legs = TestHelper.GenerateLegs(actualRoute);
            route.Legs.Returns(legs);

            // Act
            bool actual = pathSpec.IsSatisfiedBy(route);

            // Assert
            Assert.AreEqual(expectedResult, actual);
            Assert.Null(route.ReceivedWithAnyArgs().Legs);
        }

        /// <summary>
        /// Tests if it knows when A route might satisfy
        /// </summary>
        /// <param name="routeConfiguration">The route configuration.</param>
        /// <param name="citiesRoute">The cities route.</param>
        /// <param name="expectedResult">if set to <c>true</c> [expected result].</param>
        [Test]
        [TestCaseSource("testDataForMightSatisfyTest")]
        public void TestIfItKnowsWhenARouteMightSatisfy(string[] routeConfiguration, string[] citiesRoute, bool expectedResult)
        {
            // Arrange
            PathSpecification pathSpec = new PathSpecification(citiesRoute);
            IRoute route = Substitute.For<IRoute>();
            IList<IRailroad> legs = TestHelper.GenerateLegs(routeConfiguration);

            route.Legs.Returns(legs);

            // Act
            bool actual = pathSpec.MightBeSatisfiedBy(route);

            // Assert
            Assert.AreEqual(expectedResult, actual);
            Assert.Null(route.ReceivedWithAnyArgs().Legs);
        }
    }
}
