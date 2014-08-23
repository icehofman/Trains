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
using Trains.Test;

namespace Trains.IntegrationTest
{
    // StopsCountSpecification test cases
    // Satisfying
    [TestFixture(typeof(StopsCountSpecification), new string[] { "AB1" }, true, 1, 1)]
    [TestFixture(typeof(StopsCountSpecification), new string[] { "AB1", "BC1" }, true, 1, 2)]
    [TestFixture(typeof(StopsCountSpecification), new string[] { "AB1", "BC1" }, true, 1, 3)]
    //// Non satisfying
    [TestFixture(typeof(StopsCountSpecification), new string[] { "AB1", "BC1" }, false, 3, 4)]
    [TestFixture(typeof(StopsCountSpecification), new string[] { "AB4", "BC1" }, false, 3, 4)]

    // PathSpecification test cases
    // Satisfying
    [TestFixture(typeof(PathSpecification), new string[] { "AB" }, true, "A", "B")]
    [TestFixture(typeof(PathSpecification), new string[] { "AB", "BC" }, true, "A", "B", "C")]
    [TestFixture(typeof(PathSpecification), new string[] { "AB", "BA" }, true, "A", "B", "A")]
    //// Non satisfying
    [TestFixture(typeof(PathSpecification), new string[] { "AB" }, false, "A", "C")]
    [TestFixture(typeof(PathSpecification), new string[] { "AB" }, false, "C", "B")]
    [TestFixture(typeof(PathSpecification), new string[] { "AB" }, false, "A", "B", "C")]
    [TestFixture(typeof(PathSpecification), new string[] { "AB", "BC" }, false, "A", "B")]
    [TestFixture(typeof(PathSpecification), new string[] { "AB", "BD" }, false, "A", "B", "C")]
    [TestFixture(typeof(PathSpecification), new string[] { "DB", "BC" }, false, "A", "B", "C")]
    [TestFixture(typeof(PathSpecification), new string[] { "AB", "BC" }, false, "A", "B", "D")]
    [TestFixture(typeof(PathSpecification), new string[] { "AB", "BC", "CD" }, false, "A", "B", "C")]

    // DistanceSpecification test cases
    // Satisfying
    [TestFixture(typeof(DistanceSpecification), new string[] { "AB1" }, true, 0, 1)]
    [TestFixture(typeof(DistanceSpecification), new string[] { "AB1" }, true, 1, 1)]
    [TestFixture(typeof(DistanceSpecification), new string[] { "AB1" }, true, 0, 2)]
    [TestFixture(typeof(DistanceSpecification), new string[] { "AB1", "BC1" }, true, 0, 2)]
    [TestFixture(typeof(DistanceSpecification), new string[] { "AB1", "BC1" }, true, 0, 3)]
    [TestFixture(typeof(DistanceSpecification), new string[] { "AB1", "BC1" }, true, 1, 2)]
    [TestFixture(typeof(DistanceSpecification), new string[] { "AB1", "BC1" }, true, 2, 2)]
    //// Non satisfying
    [TestFixture(typeof(DistanceSpecification), new string[] { "AB2" }, false, 0, 1)]
    [TestFixture(typeof(DistanceSpecification), new string[] { "AB2" }, false, 3, 3)]
    [TestFixture(typeof(DistanceSpecification), new string[] { "AB1", "BC1" }, false, 0, 1)]
    [TestFixture(typeof(DistanceSpecification), new string[] { "AB1", "BC1" }, false, 3, 3)]

    // OriginAndEndSpecification test cases
    // Satisfying
    [TestFixture(typeof(OriginAndDestinationSpecification), "AB1", true, "A", "B")]
    [TestFixture(typeof(OriginAndDestinationSpecification), "AB1 BC1", true, "A", "C")]
    [TestFixture(typeof(OriginAndDestinationSpecification), "AB1 BC1 CD", true, "A", "D")]
    //// Non satisfying
    [TestFixture(typeof(OriginAndDestinationSpecification), "AB1", false, "A", "C")]
    [TestFixture(typeof(OriginAndDestinationSpecification), "AB1", false, "C", "B")]
    [TestFixture(typeof(OriginAndDestinationSpecification), "AB1 BD1", false, "A", "C")]
    [TestFixture(typeof(OriginAndDestinationSpecification), "AB1 BC1 CA1", false, "A", "C")]
    [TestFixture(typeof(OriginAndDestinationSpecification), "AB1 DC1 CB1", false, "C", "B")]
    public class Specification_Route_IntegrationTest<T>
        where T : class, IRouteSpecification
    {
        /// <summary>
        /// Test arguments to be used for the test case
        /// </summary>
        private object[][] testArguments;

        /// <summary>
        /// Initializes a new instance of the <see cref="Specification_Route_IntegrationTest&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="path">The graph path.</param>
        /// <param name="expectedResult">if set to <c>true</c> [expected result].</param>
        /// <param name="constructorArguments">The constructor arguments for the specification.</param>
        public Specification_Route_IntegrationTest(string path, bool expectedResult, params object[] constructorArguments)
        {
            string[] tokens = path.Split(' ');
            this.testArguments = new object[][] { new object[] { tokens, expectedResult, constructorArguments } };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="Specification_Route_IntegrationTest&lt;T&gt;"/> class.
        /// </summary>
        /// <param name="path">The graph path.</param>
        /// <param name="expectedResult">if set to <c>true</c> [expected result].</param>
        /// <param name="constructorArguments">The constructor arguments for the specification.</param>
        public Specification_Route_IntegrationTest(string[] path, bool expectedResult, params object[] constructorArguments)
        {
            this.testArguments = new object[][] { new object[] { path, expectedResult, constructorArguments } };
        }

        /// <summary>
        /// Tests if specification knows if route satisfies
        /// </summary>
        /// <param name="routePath">The route path.</param>
        /// <param name="expectedResult">if set to <c>true</c> [expected result].</param>
        /// <param name="constructorArguments">The constructor arguments.</param>
        [Test]
        [TestCaseSource("testArguments")]
        public void TestIfSpecificationKnowsIfRouteSatisfies(string[] routePath, bool expectedResult, object[] constructorArguments)
        {
            // Arrange
            IRouteSpecification spec = Substitute.For<T>(constructorArguments);
            IRoute route = Substitute.For<Route>();
            IList<IRailroad> railroads = TestHelper.GenerateLegs(routePath);
            foreach (IRailroad railroad in railroads)
            {
                route.AddLeg(railroad);
            }

            // Act
            bool actualResult = spec.IsSatisfiedBy(route);

            // Assert
            Assert.AreEqual(expectedResult, actualResult);
        }
    }    
}
