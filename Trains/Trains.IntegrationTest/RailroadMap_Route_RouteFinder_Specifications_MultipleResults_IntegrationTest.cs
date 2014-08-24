using NSubstitute;
using NUnit.Framework;
using System;
using System.Linq;
using Trains.Map;
using Trains.Plan;
using Trains.Specify;
using Trains.Test;

namespace Trains.IntegrationTest
{
    [TestFixture]
    public class RailroadMap_Route_RouteFinder_Specifications_MultipleResults_IntegrationTest
    {
        [
         TestCase( "AB1", new string[] { "AB1" }, new Type[] { typeof(OriginAndDestinationSpecification), typeof(DistanceSpecification) }, new object[] { "A", "B" }, new object[] { 1, 1 }),  
         TestCase(
             "AB1, BC1",
             new string[] { "AB1" },
             new Type[] { typeof(OriginAndDestinationSpecification), typeof(DistanceSpecification) },
             new object[] { "A", "B" }, new object[] { 0, 1 }),
         TestCase(
             "AB1, BC1",
             new string[] { "AB1, BC1" },
             new Type[] { typeof(OriginAndDestinationSpecification), typeof(DistanceSpecification) },
             new object[] { "A", "C" }, new object[] { 0, 2 }),
         TestCase(
             "AB1, BC1, CA1",
             new string[] { "AB1, BC1", "AB1, BC1, CA1, AB1, BC1" },
             new Type[] { typeof(OriginAndDestinationSpecification), typeof(DistanceSpecification) },
             new object[] { "A", "C" }, new object[] { 0, 5 }),
         TestCase(
             "AB1",
             new string[] { },
             new Type[] { typeof(OriginAndDestinationSpecification), typeof(DistanceSpecification) },
             new object[] { "A", "C" }, new object[] { 1, 1 }),
         TestCase(
             "AB1",
             new string[] { },
             new Type[] { typeof(OriginAndDestinationSpecification), typeof(DistanceSpecification) },
             new object[] { "A", "B" }, new object[] { 2, 2 }),
         TestCase(
             "AB1, BC1",
             new string[] { },
             new Type[] { typeof(OriginAndDestinationSpecification), typeof(DistanceSpecification) },
             new object[] { "A", "D" }, new object[] { 0, 3 }),
         TestCase(
             "AB1, BC1",
             new string[] { },
             new Type[] { typeof(OriginAndDestinationSpecification), typeof(DistanceSpecification) },
             new object[] { "A", "C" }, new object[] { 3, 3 }),
         TestCase(
             "AB1",
             new string[] { "AB1" },
             new Type[] { typeof(PathSpecification), typeof(StopsCountSpecification) },
             new object[] { "A", "B" }, new object[] { 0, 1 }), 
         TestCase(
             "AB1",
             new string[] { "AB1" },
             new Type[] { typeof(PathSpecification), typeof(StopsCountSpecification) },
             new object[] { "A", "B" }, new object[] { 1, 1 }),
         TestCase(
             "AB1, BC1",
             new string[] { "AB1, BC1" },
             new Type[] { typeof(PathSpecification), typeof(StopsCountSpecification) },
             new object[] { "A", "B", "C" }, new object[] { 0, 2 }),
         TestCase(
             "AB1",
             new string[] { },
             new Type[] { typeof(PathSpecification), typeof(StopsCountSpecification) },
             new object[] { "A", "C" }, new object[] { 0, 1 }),
         TestCase(
             "AB1",
             new string[] { },
             new Type[] { typeof(PathSpecification), typeof(StopsCountSpecification) },
             new object[] { "A", "B" }, new object[] { 2, 2 }),
         TestCase(
             "AB1, BC1",
             new string[] { },
             new Type[] { typeof(PathSpecification), typeof(StopsCountSpecification) },
             new object[] { "A", "B", "D" }, new object[] { 2, 2 }),
         TestCase(
             "AB1, BC1",
             new string[] { },
             new Type[] { typeof(PathSpecification), typeof(StopsCountSpecification) },
             new object[] { "A", "B", "C" }, new object[] { 0, 1 }),
         TestCase(
             "AB5, BC4, CD8, DC8, DE6, AD5, CE2, EB3, AE7",
             new string[] { "AB5, BC4, CD8, DC8", "AD5, DC8, CD8, DC8", "AD5, DE6, EB3, BC4" },
             new Type[] { typeof(OriginAndDestinationSpecification), typeof(StopsCountSpecification) },
             new object[] { "A", "C" }, new object[] { 4, 4 }),
         TestCase( "AB5, BC4, CD8, DC8, DE6, AD5, CE2, EB3, AE7", new string[] { "CD8, DC8", "CE2, EB3, BC4", "CE2, EB3, BC4, CD8, DC8", "CD8, DC8, CE2, EB3, BC4", "CD8, DE6, EB3, BC4", "CE2, EB3, BC4, CE2, EB3, BC4", "CE2, EB3, BC4, CE2, EB3, BC4, CE2, EB3, BC4" }, new Type[] { typeof(OriginAndDestinationSpecification), typeof(DistanceSpecification) }, new object[] { "C", "C" }, new object[] { 0, 29 })
        ]

        /// <summary>
        /// Tests if route finder can find all routes that comply with A composite spec
        /// </summary>
        /// <param name="graphPath">The graph path.</param>
        /// <param name="expectedRoutesPaths">The expected routes paths.</param>
        /// <param name="specificationTypes">The specification types.</param>
        /// <param name="specificationsConstructorArgs">The specifications constructor arguments.</param>
        [Test]
        public void TestIfRouteFinderCanFindAllRoutesThatComplyWithACompositeSpec( string graphPath, string[] expectedRoutesPaths, Type[] specificationTypes, params object[][] specificationsConstructorArgs)
        {
            RailroadMap map = new RailroadMap();
            map.BuildMap(graphPath);
            RouteFinder routeFinder = new RouteFinder(map);
            var specs = new IRouteSpecification[specificationTypes.Length];
            for (int i = 0; i < specificationTypes.Length; i++)
            {
                IRouteSpecification specification = Substitute.For(new Type[] { specificationTypes[i] }, specificationsConstructorArgs[i]) as IRouteSpecification;
                specs[i] = specification;
            }

            AndSpecification compositeSpecification = new AndSpecification(specs);
            Route[] results = new Route[] { };

            results = routeFinder.FindRoutes(compositeSpecification).Cast<Route>().ToArray();

            if (expectedRoutesPaths.Count() > 0)
            {
                Assert.AreEqual(expectedRoutesPaths.Count(), results.Count());

                foreach (string expectedRouteGraph in expectedRoutesPaths)
                {
                    Route expectedRoute = TestHelper.BuildRouteFromString(expectedRouteGraph);
                    CollectionAssert.Contains(results, expectedRoute);
                }
            }
            else
            {
                Assert.IsEmpty(results);
            }
        }
    }
}
