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

namespace Trains.Test.Plan
{
    [TestFixture]
    public class RouteFinderTest
    {
        /// <summary>
        /// A test count used to know the test case number.
        /// </summary>
        private int testCount = 1;

        //// Graph, Route specification, Valid route available?, Route that should be found
        //// AB1 = Stops: 1, Length: 1, Path: A-B
        [TestCase(new string[] { "AB1" }, new string[] { "AB1" }, true, new string[] { "AB1" })]
        [TestCase(new string[] { "AB1", "BC1" }, new string[] { "AB1" }, true, new string[] { "AB1" })]
        [TestCase(new string[] { "AB1", "BA1" }, new string[] { "AB1" }, true, new string[] { "AB1" })]
        [TestCase(new string[] { "AB2" }, new string[] { "AB1" }, false, new string[] { "ZZ0" })]
        [TestCase(new string[] { "AB1", "BC1", "CA1" }, new string[] { "AB1" }, true, new string[] { "AB1" })]
        //// AC1 = Stops: 1, Length: 1, Path: A-C
        [TestCase(new string[] { "AB1" }, new string[] { "AC1" }, false, new string[] { "ZZ0" })]
        //// AB1 BC1 = Stops: 2, Length: 2, Path: A-B-C
        [TestCase(new string[] { "AB1", "BC1" }, new string[] { "AB1", "BC1" }, true, new string[] { "AB1", "BC1" })]
        //// AB0 BC0 = Stops: 2, Length: Any, Path: A-B-C
        [TestCase(new string[] { "AB1", "BC2" }, new string[] { "AB0", "BC0" }, true, new string[] { "AB1", "BC2" })]
        //// AZ0 ZZ0 ZA0 = Stops: 3, Length: Any, Path: A-X-X-A
        [TestCase(new string[] { "AB1", "BC1", "CA1" }, new string[] { "AZ0", "ZZ0", "ZA0" }, true, new string[] { "AB1", "BC1", "CA1" })]
        //// AB1 BC1 = Stops: 2, Length: 2, Path: A-B-C
        [TestCase(new string[] { "AB1", "BC2" }, new string[] { "AB1", "BC1" }, false, new string[] { "ZZ0" })]
        //// AZ0 ZZ0 ZZ0 ZC0 = Stops: 4, Length: Any, Path: A-X-X-X-C
        [TestCase(
            new string[] { "AB5", "BC4", "CD8", "DC8", "DE6", "AD5", "CE2", "EB3", "AE7" }, // Graph
            new string[] { "AZ0", "ZZ0", "ZZ0", "ZC0" }, // Route Specification
            true, // Should find a valid routeFinderTest
            new string[] { "AB5", "BC4", "CD8", "DC8" })] // Route that should be found
        //// CE0 ZZ0 ZZ0 ZZ0 DC30 = Stops: 5, Length: 30 (or less), Path: C-E-X-X-D-C
        [TestCase(
            new string[] { "AB5", "BC4", "CD8", "DC8", "DE6", "AD5", "CE2", "EB3", "AE7" }, // Graph
            new string[] { "CE0", "ZZ0", "ZZ0", "ZZ0", "DC30" }, // Route specification
            true, // Should find a valid route
            new string[] { "CE2", "EB3", "BC4", "CD8", "DC8" })] // Route that should be found
        //// AE0 ED0 = Stops: 2, Length: Any, Path: A-E-D
        [TestCase(
            new string[] { "AB5", "BC4", "CD8", "DC8", "DE6", "AD5", "CE2", "EB3", "AE7" }, // Graph
            new string[] { "AE0", "ED0" }, // Route Specification
            false, // Should find a valid route
            new string[] { "ZZ0" })] // ZZ0 is ignored.

        /// <summary>
        /// Tests if it can find first satisfying route
        /// </summary>
        /// <param name="graph">The graph of the map.</param>
        /// <param name="specifiedRoute">The specified route.</param>
        /// <param name="shouldFindAValidRoute">if set to <c>true</c> [should find a valid route].</param>
        /// <param name="expectedRouteGraph">The expected route graph. Ignored if <paramref name="shouldFindAValidRoute"/> is set to false</param>
        public void TestIfItCanFindFirstSatisfyingRoute(
            string[] graph,
            string[] specifiedRoute,
            bool shouldFindAValidRoute,
            string[] expectedRouteGraph)
        {
            // Arrange
            // Arrange the expected route
            IRoute expectedRoute = TestHelper.BuildMockRoute(expectedRouteGraph);

            // Arrange the map
            IRailroadMap map = Substitute.For<IRailroadMap>();
            IList<ICity> cities = TestHelper.GenerateCities(graph);
            IList<IRailroad> railroads = TestHelper.GenerateLegs(graph, cities);
            map.Cities.Returns(cities);
            map.Railroads.Returns(railroads);

            // Arrange the specification
            IRouteSpecification specification = Substitute.For<IRouteSpecification>();
            IRoute routeSpec = TestHelper.BuildMockRoute(specifiedRoute);
            specification
                .IsSatisfiedBy(null)
                .ReturnsForAnyArgs(method => FinderTestHelper.SatisfiesSpecification(routeSpec, method.Arg<IRoute>()));

            // Arrange the specification that "might" be satisfied
            IRoute routeMightBeSpec = TestHelper.BuildMockRoute(specifiedRoute);
            specification
                .MightBeSatisfiedBy(null)
                .ReturnsForAnyArgs(method => FinderTestHelper.MightSatisfySpecification(routeMightBeSpec, method.Arg<IRoute>()));

            // Arrange the target
            IRouteFinder target = Substitute.For<RouteFinder>(map);

            // Arrange root route
            IRoute rootRoute = Substitute.For<IRoute>();
            rootRoute.Legs.Returns(new List<IRailroad>());

            // Act
            IRoute actualResult = target.FindFirstSatisfyingRoute(specification);

            // Assert
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

        [TestCase(
            new string[] { "AB1" }, // Graph
            //// AB1 = Stops: 1, Length: 1, Path: A-B
            new string[] { "AB1" }, // Specified route
            true, // should find a valid route
            new object[]
            { 
                new string[] { "AB1" } // routes to find
            })]
        [TestCase(
            new string[] { "AB1" }, // Graph
            //// AB0 = Stops: 1, Length: Any, Path: A-B
            new string[] { "AB0" }, // Specified route
            true, // should find a valid route
            new object[]
            { 
                new string[] { "AB1" } // routes to find
            })]
        [TestCase(
            new string[] { "AB1" }, // Graph
            //// AC1 = Stops: 1, Length: 1, Path: A-C
            new string[] { "AC1" }, // Specified route
            false, // should find a valid route
            new object[] { })]
        [TestCase(
            new string[] { "AB3" }, // Graph
            //// AB2 = Stops: 1, Length: 2, Path: A-B
            new string[] { "AB2" }, // Specified route
            false, // should find a valid route
            new object[] { })]
        [TestCase(
            new string[] { "AB1" }, // Graph
            //// CB1 = Stops: 1, Length: 1, Path: C-B
            new string[] { "CB1" }, // Specified route
            false, // should find a valid route
            new object[] { })]
        [TestCase(
            new string[] { "AB1" }, // Graph
            //// AB0 BC0 = Stops: 2, Length: Any, Path: A-B-C
            new string[] { "AB0", "BC0" }, // Specified route
            false, // should find a valid route
            new object[] { })]
        [TestCase(
            new string[] { "AB1", "BC1" }, // Graph
            //// BC1 = Stops: 1, Length: 1, Path: B-C
            new string[] { "BC1" }, // Specified route
            true, // should find a valid route
            new object[]
            { 
                new string[] { "BC1" } // routes to find
            })]
        [TestCase(
            new string[] { "AB1", "BC1" }, // Graph
            //// AB1 BC1 = Stops: 2, Length: 2, Path: A-B-C
            new string[] { "AB1", "BC1" }, // Specified route
            true, // should find a valid route
            new object[]
            { 
                new string[] { "AB1", "BC1" } // routes to find
            })]
        [TestCase(
            new string[] { "AB1", "BC1" }, // Graph
            //// AB0 BC0 = Stops: 2, Length: Any, Path: A-B-C
            new string[] { "AB0", "BC0" }, // Specified route
            true, // should find a valid route
            new object[]
            { 
                new string[] { "AB1", "BC1" } // routes to find
            })]
        [TestCase(
            new string[] { "AB1", "BC1" }, // Graph
            //// AB0 BC1 = Stops: 2, Length: 1, Path: A-B-C
            new string[] { "AB0", "BC1" }, // Specified route
            false, // should find a valid route
            new object[] { })]
        [TestCase(
            new string[] { "AB1", "BC1" }, // Graph
            //// AB0 BD0 = Stops: 2, Length: Any, Path: A-B-D
            new string[] { "AB0", "BD0" }, // Specified route
            false, // should find a valid route
            new object[] { })]
        [TestCase(
            new string[] { "AB1", "BC1" }, // Graph
            //// AB0 BC0 CD0 = Stops: 3, Length: Any, Path: A-B-C-D
            new string[] { "AB0", "BC0", "CD0" }, // Specified route
            false, // should find a valid route
            new object[] { })]
        [TestCase(
            new string[] { "AB1", "BC1" }, // Graph
            //// CB0 BC0 = Stops: 2, Length: Any, Path: C-B-C
            new string[] { "CB0", "BC0" }, // Specified route
            false, // should find a valid route
            new object[] { })]
        [TestCase(
            new string[] { "AB1", "BC1", "CA1", "AD1", "DC1" }, // Graph
            //// AZ0 ZC0 = Stops: 2, Length: Any, Path: A-X-C
            new string[] { "AZ0", "ZC0" }, // Specified route
            true, // should find a valid route
            new object[] {
                new string[] { "AB1", "BC1" },
                new string[] { "AD1", "DC1" }
            })]
        [TestCase(
            new string[] { "AB1", "BC1", "CA1", "BD1", "DA1" }, // Graph
            //// AZ0 ZA0 = Stops: 2, Length: Any, Path: B-X-A
            new string[] { "BZ0", "ZA0" }, // Specified route
            true, // should find a valid route
            new object[] {
                new string[] { "BC1", "CA1" },
                new string[] { "BD1", "DA1" }
            })]
        [TestCase(
            new string[] { "AB5", "BC4", "CD8", "DC8", "DE6", "AD5", "CE2", "EB3", "AE7" }, // Graph
            new string[] { "AZ0", "ZZ0", "ZZ0", "ZC0" }, // Route Specification
            true, // Should find a valid route
            new object[] { 
                new string[] { "AB5", "BC4", "CD8", "DC8" },
                new string[] { "AD5", "DC8", "CD8", "DC8" },
                new string[] { "AD5", "DE6", "EB3", "BC4" }
            })]

        /// <summary>
        /// Tests if it can find all satisfying routes
        /// </summary>
        /// <param name="graph">The graph.</param>
        /// <param name="specifiedRoute">The specified route.</param>
        /// <param name="shouldFindAValidRoute">if set to <c>true</c> [should find A valid route].</param>
        /// <param name="expectedRoutes">The expected routes found.</param>
        [Test]
        public void TestIfItCanFindAllSatisfyingRoutes(
            string[] graph,
            string[] specifiedRoute,
            bool shouldFindAValidRoute,
            object[] expectedRoutes)
        {
            string graphMsg = graph.Aggregate((acum, item) => string.Format("{0} {1}", acum, item));
            string specifiedRouteMsg = specifiedRoute.Aggregate((acum, item) => string.Format("{0} {1}", acum, item));
            string findValidRouteMsg = shouldFindAValidRoute.ToString();
            Console.WriteLine("#{0} Graph:{1} SpecifiedRoute:{2} Valid Route:{3}", this.testCount++, graphMsg, specifiedRouteMsg, findValidRouteMsg);

            // Arrange

            // Arrange the map
            IRailroadMap map = Substitute.For<IRailroadMap>();
            IList<ICity> cities = TestHelper.GenerateCities(graph);
            IList<IRailroad> railroads = TestHelper.GenerateLegs(graph, cities);
            map.Cities.Returns(cities);
            map.Railroads.Returns(railroads);

            // Arrange the specification
            IRouteSpecification specification = Substitute.For<IRouteSpecification>();
            IRoute routeSpec = TestHelper.BuildMockRoute(specifiedRoute);
            specification
                .IsSatisfiedBy(null)
                .ReturnsForAnyArgs(method => FinderTestHelper.SatisfiesSpecification(routeSpec, method.Arg<IRoute>()));

            // Arrange the specification that "might" be satisfied
            IRoute routeMightBeSpec = TestHelper.BuildMockRoute(specifiedRoute);
            specification
                .MightBeSatisfiedBy(null)
                .ReturnsForAnyArgs(method => FinderTestHelper.MightSatisfySpecification(routeMightBeSpec, method.Arg<IRoute>()));

            // Arrange the target
            IRouteFinder target = Substitute.For<RouteFinder>(map);

            // Arrange root route
            IRoute rootRoute = Substitute.For<IRoute>();
            rootRoute.Legs.Returns(new List<IRailroad>());

            // Act
            IEnumerable<IRoute> actualResult = target.FindRoutes(specification);

            // Assert
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
