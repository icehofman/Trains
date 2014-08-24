using NSubstitute;
using NUnit.Framework;
using System;
using Trains.Map;
using Trains.Plan;
using Trains.Specify;
using Trains.Test;

namespace Trains.IntegrationTest
{
    [TestFixture]
    public class RailroadMap_RouteFinder_ISpecificationImplementations_SingleResults_IntegrationTest
    {
        /// <summary>
        /// Test parameter to define a route was not found.
        /// </summary>
        private const string NONE = "";

        [
         TestCase(typeof(OriginAndDestinationSpecification), "AB1", "AB1", "A", "B"),
         TestCase(typeof(OriginAndDestinationSpecification), "AB1, BC1", "AB1", "A", "B"),
         TestCase(typeof(OriginAndDestinationSpecification), "AB1, BC1", "BC1", "B", "C"),
         TestCase(typeof(OriginAndDestinationSpecification), "AB1, BC1", "BC1", "B", "C"),
         TestCase(typeof(OriginAndDestinationSpecification), "AB1, BC1", "AB1, BC1", "A", "C"),
         TestCase(typeof(OriginAndDestinationSpecification), "AB1, BC1, CD1", "BC1", "B", "C"),
         TestCase(typeof(OriginAndDestinationSpecification), "AB1, BC1, CD1", "BC1, CD1", "B", "D"),
         TestCase(typeof(OriginAndDestinationSpecification), "AB1, BC1, CD1, DE1", "BC1, CD1", "B", "D"),
         TestCase(typeof(OriginAndDestinationSpecification), "AB1", NONE, "A", "C"),
         TestCase(typeof(OriginAndDestinationSpecification), "AB1", NONE, "B", "B"),
         TestCase(typeof(OriginAndDestinationSpecification), "AB1, BC1", NONE, "A", "D"),
         TestCase(typeof(OriginAndDestinationSpecification), "AB1, BC1, ED1", NONE, "A", "D"),
         TestCase(typeof(OriginAndDestinationSpecification), "AB1, BC1", NONE, "B", "D")
        ]

        [
         TestCase(typeof(DistanceSpecification), "AB1", "AB1", 0, 1),
         TestCase(typeof(DistanceSpecification), "AB1", "AB1", 1, 1),
         TestCase(typeof(DistanceSpecification), "AB1, BC1", "AB1", 0, 1),
         TestCase(typeof(DistanceSpecification), "AB1, BC1", "AB1", 1, 1),
         TestCase(typeof(DistanceSpecification), "AB2, BC1", "BC1", 0, 1),
         TestCase(typeof(DistanceSpecification), "AB2, BC1", "BC1", 1, 1),
         TestCase(typeof(DistanceSpecification), "AB1, BC1", "AB1", 0, 2),
         TestCase(typeof(DistanceSpecification), "AB1, BC1", "AB1", 1, 2),
         TestCase(typeof(DistanceSpecification), "AB1, BC1", "AB1, BC1", 2, 2),
         TestCase(typeof(DistanceSpecification), "AB3, BC1", "BC1", 0, 2),
         TestCase(typeof(DistanceSpecification), "AB3, BC1", "BC1", 1, 2),
         TestCase(typeof(DistanceSpecification), "AB1, BC1, CD1", "AB1, BC1", 2, 2),
         TestCase(typeof(DistanceSpecification), "AB3, BC1, CD1", "BC1, CD1", 2, 2),
         TestCase(typeof(DistanceSpecification), "AB1, BC1, CA1", "AB1, BC1, CA1, AB1", 4, 4),
         TestCase(typeof(DistanceSpecification), "CD7, AB1, BC1, CA1, DA7", "CA1, AB1, BC1, CA1, AB1, BC1", 6, 6),
         TestCase(typeof(DistanceSpecification), "AD7, AB1, BC1, CA1, DA7", "AB1, BC1, CA1, AB1, BC1, CA1", 6, 6),
         TestCase(typeof(DistanceSpecification), "AB1", NONE, 0, 0),
         TestCase(typeof(DistanceSpecification), "AB2", NONE, 0, 1),
         TestCase(typeof(DistanceSpecification), "AB1", NONE, 2, 3),
         TestCase(typeof(DistanceSpecification), "AB3", NONE, 0, 2),
         TestCase(typeof(DistanceSpecification), "AB1, BC1", NONE, 0, 0),
         TestCase(typeof(DistanceSpecification), "AB1, BC1", NONE, 3, 4),
         TestCase(typeof(DistanceSpecification), "AB1, BC1, DC1", NONE, 3, 4),
         TestCase(typeof(DistanceSpecification), "AB3, BC1", NONE, 2, 2)
        ]

        [
         TestCase(typeof(PathSpecification), "AB1", "AB1", "A", "B"),
         TestCase(typeof(PathSpecification), "AB1, BC1", "AB1", "A", "B"),
         TestCase(typeof(PathSpecification), "AB1, BC1", "BC1", "B", "C"),
         TestCase(typeof(PathSpecification), "AB1, BC1, CD1", "BC1", "B", "C"),
         TestCase(typeof(PathSpecification), "AB1, BC1, CD1", "AB1, BC1", "A", "B", "C"),
         TestCase(typeof(PathSpecification), "AB1, BC1, CD1", "BC1, CD1", "B", "C", "D"),
         TestCase(typeof(PathSpecification), "AB1, BC1, CD1, DE1", "BC1, CD1", "B", "C", "D"),
         TestCase(typeof(PathSpecification), "AB1", NONE, "A", "C"),
         TestCase(typeof(PathSpecification), "AC1", NONE, "B", "C"),
         TestCase(typeof(PathSpecification), "AB1", NONE, "A", "B", "C"),
         TestCase(typeof(PathSpecification), "AB1, BC1", NONE, "A", "D"),
         TestCase(typeof(PathSpecification), "AB1, BC1", NONE, "D", "B"),
         TestCase(typeof(PathSpecification), "AB1, BC1", NONE, "A", "B", "C", "D")
        ]

        [
         TestCase(typeof(StopsCountSpecification), "AB1", "AB1", 0, 1),
         TestCase(typeof(StopsCountSpecification), "AB1", "AB1", 1, 1),
         TestCase(typeof(StopsCountSpecification), "AB1, BC1", "AB1", 1, 1),
         TestCase(typeof(StopsCountSpecification), "AB1, BC1", "AB1", 1, 2),
         TestCase(typeof(StopsCountSpecification), "AB1, BC1", "AB1", 0, 2),
         TestCase(typeof(StopsCountSpecification), "AB1, BC1", "AB1, BC1", 2, 2),
         TestCase(typeof(StopsCountSpecification), "AB1", NONE, 0, 0),
         TestCase(typeof(StopsCountSpecification), "AB1", NONE, 2, 2),
         TestCase(typeof(StopsCountSpecification), "AB1, CD1", NONE, 2, 2),
         TestCase(typeof(StopsCountSpecification), "AB1", NONE, 2, 3),
         TestCase(typeof(StopsCountSpecification), "AB1, BC1", NONE, 3, 3),
         TestCase(typeof(StopsCountSpecification), "AB1, BC1", NONE, 3, 4),
         TestCase(typeof(StopsCountSpecification), "AB1, BC1, DC1, DE1", NONE, 4, 4)
        ]

        /// <summary>
        /// Tests if route finder execute simple single result specifications
        /// </summary>
        /// <param name="specificationType">The type of the specificaiton to create.</param>
        /// <param name="graph">The railroad map graph.</param>
        /// <param name="expectedRouteGraph">The expected route.</param>
        /// <param name="specCtorArgs">The specification constructor arguments.</param>
        [Test]
        public void TestIfRouteFinderExecuteSimpleSingleResultSpecifications(Type specificationType, string graph, string expectedRouteGraph, params object[] specCtorArgs)
        {
            RailroadMap map = new RailroadMap();
            map.BuildMap(graph);
            RouteFinder routeFinder = new RouteFinder(map);
            IRouteSpecification specification = Substitute.For(new Type[] { specificationType }, specCtorArgs) as IRouteSpecification;
            IRoute expectedRoute = TestHelper.BuildRouteFromString(expectedRouteGraph);
            IRoute actualRoute = default(IRoute);

            actualRoute = routeFinder.FindFirstSatisfyingRoute(specification);

            Assert.AreEqual(expectedRoute.ToString(), actualRoute == null ? new Route().ToString() : actualRoute.ToString());
        }

        [
         TestCase( "AB1", "AB1", new Type[] { typeof(PathSpecification), typeof(DistanceSpecification) }, new object[] { "A", "B" }, new object[] { 0, 1 }),
         TestCase( "AB1, BC1", "AB1", new Type[] { typeof(PathSpecification), typeof(DistanceSpecification) }, new object[] { "A", "B" }, new object[] { 1, 1 }),
         TestCase( "AB1, BC1", "AB1, BC1", new Type[] { typeof(PathSpecification), typeof(DistanceSpecification) }, new object[] { "A", "B", "C" }, new object[] { 0, 2 }),
         TestCase( "AC1", NONE, new Type[] { typeof(PathSpecification), typeof(DistanceSpecification) }, new object[] { "A", "B" }, new object[] { 1, 1 }),
         TestCase( "AB2", NONE, new Type[] { typeof(PathSpecification), typeof(DistanceSpecification) }, new object[] { "A", "B" }, new object[] { 1, 1 }),
         TestCase( "AB1, BC1", NONE, new Type[] { typeof(PathSpecification), typeof(DistanceSpecification) }, new object[] { "A", "B", "C" }, new object[] { 0, 1 }),
         TestCase( "AB1, BC1", NONE, new Type[] { typeof(PathSpecification), typeof(DistanceSpecification) }, new object[] { "A", "B", "D" }, new object[] { 0, 2 })
        ]

        [
         TestCase( "AB1", "AB1", new Type[] { typeof(OriginAndDestinationSpecification), typeof(StopsCountSpecification) }, new object[] { "A", "B" }, new object[] { 0, 1 }),
         TestCase( "AB1, BC1", "AB1", new Type[] { typeof(OriginAndDestinationSpecification), typeof(StopsCountSpecification) }, new object[] { "A", "B" }, new object[] { 1, 1 }),
         TestCase( "AB1, BC1", "AB1, BC1", new Type[] { typeof(OriginAndDestinationSpecification), typeof(StopsCountSpecification) }, new object[] { "A", "C" }, new object[] { 0, 2 }),
         TestCase( "AB1", NONE, new Type[] { typeof(OriginAndDestinationSpecification), typeof(StopsCountSpecification) }, new object[] { "A", "C" }, new object[] { 0, 1 }),
         TestCase( "AB1", NONE, new Type[] { typeof(OriginAndDestinationSpecification), typeof(StopsCountSpecification) }, new object[] { "A", "B" }, new object[] { 2, 2 }),
         TestCase( "AB1, BC1", NONE, new Type[] { typeof(OriginAndDestinationSpecification), typeof(StopsCountSpecification) }, new object[] { "A", "D" }, new object[] { 0, 2 }),
         TestCase( "AB1, BC1", NONE, new Type[] { typeof(OriginAndDestinationSpecification), typeof(StopsCountSpecification) }, new object[] { "A", "C" }, new object[] { 3, 3 })
        ]

        /// <summary>
        /// Tests if route finder executes compound single result specifications
        /// </summary>
        /// <param name="graph">The railroad map graph.</param>
        /// <param name="expectedRouteGraph">The expected route graph.</param>
        /// <param name="specificationTypes">The specification types.</param>
        /// <param name="specsCtorsArgs">The specifications constructors arguments.</param>
        [Test]
        public void TestIfRouteFinderExecutesCompoundSingleResultSpecifications(string graph, string expectedRouteGraph, Type[] specificationTypes, params object[][] specsCtorsArgs)
        {
            RailroadMap map = new RailroadMap();
            map.BuildMap(graph);
            RouteFinder routeFinder = new RouteFinder(map);
            var specs = new IRouteSpecification[specificationTypes.Length];
            for (int i = 0; i < specificationTypes.Length; i++)
            {
                IRouteSpecification specification = Substitute.For(new Type[] { specificationTypes[i] }, specsCtorsArgs[i]) as IRouteSpecification;
                specs[i] = specification;
            }

            AndSpecification compositeSpecification = new AndSpecification(specs);
            IRoute expectedRoute = TestHelper.BuildRouteFromString(expectedRouteGraph);
            IRoute actualRoute = default(IRoute);

            actualRoute = routeFinder.FindFirstSatisfyingRoute(compositeSpecification);

            Assert.AreEqual(expectedRoute.ToString(), actualRoute == null ? new Route().ToString() : actualRoute.ToString());
        }
    }
}
