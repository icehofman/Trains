using NSubstitute;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using Trains.Map;
using Trains.Plan;
using Trains.Specify;

namespace Trains.Test.Plan
{
    [TestFixture]
    public class RouteFinderTest
    {
        private int testCount = 1;

        [TestCase(new string[] { "AB1" }, new string[] { "AB1" }, true, new string[] { "AB1" })]

        [TestCase(new string[] { "AB1", "BC1" }, new string[] { "AB1" }, true, new string[] { "AB1" })]

        [TestCase(new string[] { "AB1", "BA1" }, new string[] { "AB1" }, true, new string[] { "AB1" })]

        [TestCase(new string[] { "AB2" }, new string[] { "AB1" }, false, new string[] { "ZZ0" })]
        [TestCase(new string[] { "AB1", "BC1", "CA1" }, new string[] { "AB1" }, true, new string[] { "AB1" })]


        [TestCase(new string[] { "AB1" }, new string[] { "AC1" }, false, new string[] { "ZZ0" })]

        [TestCase(new string[] { "AB1", "BC1" }, new string[] { "AB1", "BC1" }, true, new string[] { "AB1", "BC1" })]

        [TestCase(new string[] { "AB1", "BC2" }, new string[] { "AB0", "BC0" }, true, new string[] { "AB1", "BC2" })]

        [TestCase(new string[] { "AB1", "BC1", "CA1" }, new string[] { "AZ0", "ZZ0", "ZA0" }, true, new string[] { "AB1", "BC1", "CA1" })]

        [TestCase(new string[] { "AB1", "BC2" }, new string[] { "AB1", "BC1" }, false, new string[] { "ZZ0" })]

        [TestCase(new string[] { "AB5", "BC4", "CD8", "DC8", "DE6", "AD5", "CE2", "EB3", "AE7" }, new string[] { "AZ0", "ZZ0", "ZZ0", "ZC0" }, true, new string[] { "AB5", "BC4", "CD8", "DC8" })]

        [TestCase(new string[] { "AB5", "BC4", "CD8", "DC8", "DE6", "AD5", "CE2", "EB3", "AE7" }, new string[] { "CE0", "ZZ0", "ZZ0", "ZZ0", "DC30" }, true, new string[] { "CE2", "EB3", "BC4", "CD8", "DC8" })]

        [TestCase(new string[] { "AB5", "BC4", "CD8", "DC8", "DE6", "AD5", "CE2", "EB3", "AE7" },new string[] { "AE0", "ED0" }, false, new string[] { "ZZ0" })] 

        /// <summary>
        /// Tests if it can find first satisfying route
        /// </summary>
        /// <param name="graph">The graph of the map.</param>
        /// <param name="specifiedRoute">The specified route.</param>
        /// <param name="shouldFindAValidRoute">if set to <c>true</c> [should find a valid route].</param>
        /// <param name="expectedRouteGraph">The expected route graph. Ignored if <paramref name="shouldFindAValidRoute"/> is set to false</param>
        public void TestIfItCanFindFirstSatisfyingRoute( string[] graph, string[] specifiedRoute, bool shouldFindAValidRoute, string[] expectedRouteGraph)
        {
            IRoute expectedRoute = TestHelper.BuildMockRoute(expectedRouteGraph);

            IRailroadMap map = Substitute.For<IRailroadMap>();
            IList<ICity> cities = TestHelper.GenerateCities(graph);
            IList<IRailroad> railroads = TestHelper.GenerateLegs(graph, cities);
            map.Cities.Returns(cities);
            map.Railroads.Returns(railroads);

            IRouteSpecification specification = Substitute.For<IRouteSpecification>();
            IRoute routeSpec = TestHelper.BuildMockRoute(specifiedRoute);
            specification.IsSatisfiedBy(null).ReturnsForAnyArgs(method => FinderTestHelper.SatisfiesSpecification(routeSpec, method.Arg<IRoute>()));

            IRoute routeMightBeSpec = TestHelper.BuildMockRoute(specifiedRoute);
            specification.MightBeSatisfiedBy(null).ReturnsForAnyArgs(method => FinderTestHelper.MightSatisfySpecification(routeMightBeSpec, method.Arg<IRoute>()));

            IRouteFinder target = Substitute.For<RouteFinder>(map);

            IRoute rootRoute = Substitute.For<IRoute>();
            rootRoute.Legs.Returns(new List<IRailroad>());

            IRoute actualResult = target.FindFirstSatisfyingRoute(specification);

            if (shouldFindAValidRoute)
            {
                Assert.IsNotNull(actualResult);
                Assert.IsTrue(FinderTestHelper.SatisfiesSpecification(expectedRoute, actualResult));
            }
            else
            {
                Assert.IsNull(actualResult);
            }
        }

        [TestCase( new string[] { "AB1" }, new string[] { "AB1" }, true, new object[] { new string[] { "AB1" } })]

        [TestCase( new string[] { "AB1" }, new string[] { "AB0" }, true, new object[] { new string[] { "AB1" } })]

        [TestCase( new string[] { "AB1" }, new string[] { "AC1" }, false, new object[] { })]

        [TestCase( new string[] { "AB3" }, new string[] { "AB2" }, false, new object[] { })]

        [TestCase( new string[] { "AB1" }, new string[] { "CB1" }, false, new object[] { })]

        [TestCase( new string[] { "AB1" }, new string[] { "AB0", "BC0" },  false, new object[] { })]

        [TestCase( new string[] { "AB1", "BC1" }, new string[] { "BC1" }, true, new object[] { new string[] { "BC1" } })]

        [TestCase( new string[] { "AB1", "BC1" }, new string[] { "AB1", "BC1" }, true, new object[] { new string[] { "AB1", "BC1" } })]

        [TestCase( new string[] { "AB1", "BC1" }, new string[] { "AB0", "BC0" },  true, new object[] { new string[] { "AB1", "BC1" } })]

        [TestCase( new string[] { "AB1", "BC1" }, new string[] { "AB0", "BC1" }, false, new object[] { })]

        [TestCase( new string[] { "AB1", "BC1" }, new string[] { "AB0", "BD0" }, false, new object[] { })]

        [TestCase( new string[] { "AB1", "BC1" }, new string[] { "AB0", "BC0", "CD0" }, false, new object[] { })]

        [TestCase( new string[] { "AB1", "BC1" }, new string[] { "CB0", "BC0" }, false, new object[] { })]

        [TestCase( new string[] { "AB1", "BC1", "CA1", "AD1", "DC1" }, new string[] { "AZ0", "ZC0" }, true, new object[] { new string[] { "AB1", "BC1" }, new string[] { "AD1", "DC1" } })]

        [TestCase( new string[] { "AB1", "BC1", "CA1", "BD1", "DA1" }, new string[] { "BZ0", "ZA0" }, true, new object[] { new string[] { "BC1", "CA1" }, new string[] { "BD1", "DA1" } })]

        [TestCase( new string[] { "AB5", "BC4", "CD8", "DC8", "DE6", "AD5", "CE2", "EB3", "AE7" }, new string[] { "AZ0", "ZZ0", "ZZ0", "ZC0" }, true, new object[] { new string[] { "AB5", "BC4", "CD8", "DC8" }, new string[] { "AD5", "DC8", "CD8", "DC8" }, new string[] { "AD5", "DE6", "EB3", "BC4" } })]

        /// <summary>
        /// Tests if it can find all satisfying routes
        /// </summary>
        /// <param name="graph">The graph.</param>
        /// <param name="specifiedRoute">The specified route.</param>
        /// <param name="shouldFindAValidRoute">if set to <c>true</c> [should find A valid route].</param>
        /// <param name="expectedRoutes">The expected routes found.</param>
        [Test]
        public void TestIfItCanFindAllSatisfyingRoutes( string[] graph, string[] specifiedRoute, bool shouldFindAValidRoute, object[] expectedRoutes)
        {
            string graphMsg = graph.Aggregate((acum, item) => string.Format("{0} {1}", acum, item));
            string specifiedRouteMsg = specifiedRoute.Aggregate((acum, item) => string.Format("{0} {1}", acum, item));
            string findValidRouteMsg = shouldFindAValidRoute.ToString();
            Console.WriteLine("#{0} Graph:{1} SpecifiedRoute:{2} Valid Route:{3}", this.testCount++, graphMsg, specifiedRouteMsg, findValidRouteMsg);

            IRailroadMap map = Substitute.For<IRailroadMap>();
            IList<ICity> cities = TestHelper.GenerateCities(graph);
            IList<IRailroad> railroads = TestHelper.GenerateLegs(graph, cities);
            map.Cities.Returns(cities);
            map.Railroads.Returns(railroads);

            IRouteSpecification specification = Substitute.For<IRouteSpecification>();
            IRoute routeSpec = TestHelper.BuildMockRoute(specifiedRoute);
            specification.IsSatisfiedBy(null).ReturnsForAnyArgs(method => FinderTestHelper.SatisfiesSpecification(routeSpec, method.Arg<IRoute>()));

            IRoute routeMightBeSpec = TestHelper.BuildMockRoute(specifiedRoute);
            specification.MightBeSatisfiedBy(null).ReturnsForAnyArgs(method => FinderTestHelper.MightSatisfySpecification(routeMightBeSpec, method.Arg<IRoute>()));

            IRouteFinder target = Substitute.For<RouteFinder>(map);

            IRoute rootRoute = Substitute.For<IRoute>();
            rootRoute.Legs.Returns(new List<IRailroad>());

            IEnumerable<IRoute> actualResult = target.FindRoutes(specification);

            if (shouldFindAValidRoute)
            {
                Assert.IsNotNull(actualResult);
                Assert.IsTrue(actualResult.All(route => FinderTestHelper.SatisfiesSpecification(routeSpec, route)));
                IList<IRoute> results = actualResult.ToList();
                foreach (string[] expectedRouteGraph in expectedRoutes)
                {
                    IRoute expectedRoute = TestHelper.BuildMockRoute(expectedRouteGraph);
                    IRoute foundRoute = results.FirstOrDefault(route => FinderTestHelper.SatisfiesSpecification(expectedRoute, route));
                    Assert.IsNotNull(foundRoute);
                    Assert.IsTrue(results.Remove(foundRoute));
                }
            }
            else
            {
                Assert.IsEmpty(actualResult);
            }

            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("#{0}: --- Test passed ---", this.testCount - 1);
            Console.ResetColor();
        }
    }
}
