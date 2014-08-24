using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Trains.Map;
using Trains.Plan;
using Trains.Specify;

namespace Trains
{
    public class Program
    {
        /// <summary>
        /// The format for the console messages.
        /// </summary>
        private const string OUTPUT_MESSAGE_FORMAT = "Output #{0}: {1}";

        /// <summary>
        /// Represents a operation resulting in the NO_SUCH_ROUTE MESSAGE
        /// </summary>
        private const int NO_SUCH_ROUTE = -1;

        /// <summary>
        /// The output count for the console messages format.
        /// </summary>
        private static int outputCount = 1;

        /// <summary>
        /// Prevents a default instance of the <see cref="Program"/> class from being created.
        /// </summary>
        private Program()
        {
        }

        /// <summary>
        /// Main entry point for the application
        /// </summary>
        /// <param name="args">The console arguments.</param>
        public static void Main(string[] args)
        {
            string filename = string.Empty;
            if (args.Length > 0)
            {
                filename = args[0];
            }
            else
            {
                Console.WriteLine("Please specify a filename argument.");
                return;
            }

            RailroadMap map = new RailroadMap();
            if (!new FileInfo(filename).Exists)
            {
                Console.WriteLine("The specified file: {0} does not exist.", filename);
                return;
            }

            try
            {                
                using (FileStream fileStream = File.OpenRead(filename))
                {
                    map.Init(fileStream);
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                Console.WriteLine("An error ocurred while trying to read the specified file: {0}", filename);
                return;
            }
            
            outputCount = 1;

            try
            {
                RouteFinder routeFinder = new RouteFinder(map);

                WriteOperationsResults(RunPathSpecificationUseCases(routeFinder));

                WriteOperationsResults(RunRouteCountUseCases(routeFinder));

                WriteOperationsResults(RunShortestRouteUseCases(map));

                WriteOperationsResults(RunCompoundSpecificationCountRoutesUseCase(routeFinder));
            }
            catch
            {
                Console.WriteLine("An error ocurred while trying to find routes.");
            }

            Console.Write(" ========== ");
        }

        /// <summary>
        /// Runs the compound specification count routes use case.
        /// </summary>
        /// <param name="routeFinder">The route finder.</param>
        /// <returns>The operations results</returns>
        public static IEnumerable<int> RunCompoundSpecificationCountRoutesUseCase(IRouteFinder routeFinder)
        {
            var anotherTripCountSpecs = new List<IRouteSpecification>();
            anotherTripCountSpecs.Add( new AndSpecification( new OriginAndDestinationSpecification("C", "C"), new DistanceSpecification(0, 29)));
            return FindConformingRouteCount(routeFinder, anotherTripCountSpecs);
        }

        /// <summary>
        /// Runs the shortest route use cases.
        /// </summary>
        /// <param name="map">The map.</param>
        /// <returns>
        /// The operations results
        /// </returns>
        public static IEnumerable<int> RunShortestRouteUseCases(IRailroadMap map)
        {
            var calculator = new ShortestLengthFinder(map);
            int result = calculator.GetShortestLength(from: "A", to: "C");
            yield return result == ShortestLengthFinder.Unreachable ? NO_SUCH_ROUTE : result;

            result = calculator.GetShortestLength(from: "B", to: "B");
            yield return result == ShortestLengthFinder.Unreachable ? NO_SUCH_ROUTE : result;
        }

        /// <summary>
        /// Runs the route count use cases.
        /// </summary>
        /// <param name="routeFinder">The route finder.</param>
        /// <returns>The operations results</returns>
        public static IEnumerable<int> RunRouteCountUseCases(IRouteFinder routeFinder)
        {
            var tripCountSpecs = new List<IRouteSpecification>();
            tripCountSpecs.Add( new AndSpecification( new OriginAndDestinationSpecification("C", "C"), new StopsCountSpecification(0, 3)));
            tripCountSpecs.Add( new AndSpecification( new OriginAndDestinationSpecification("A", "C"), new StopsCountSpecification(4, 4)));
            return FindConformingRouteCount(routeFinder, tripCountSpecs);
        }

        /// <summary>
        /// Runs the path specification use cases.
        /// </summary>
        /// <param name="routeFinder">The route finder.</param>
        /// <returns>The operations results</returns>
        public static IEnumerable<int> RunPathSpecificationUseCases(IRouteFinder routeFinder)
        {
            var pathSpecs = new List<IRouteSpecification>();
            pathSpecs.Add(new PathSpecification("A", "B", "C"));
            pathSpecs.Add(new PathSpecification("A", "D"));
            pathSpecs.Add(new PathSpecification("A", "D", "C"));
            pathSpecs.Add(new PathSpecification("A", "E", "B", "C", "D"));
            pathSpecs.Add(new PathSpecification("A", "E", "D"));

            return FindFirstConformingRouteDistance(routeFinder, pathSpecs);
        }

        /// <summary>
        /// Finds the conforming route counts.
        /// </summary>
        /// <param name="finder">The route finder.</param>
        /// <param name="specifications">The route specifications.</param>
        /// <returns>The operations results</returns>
        public static IEnumerable<int> FindConformingRouteCount(IRouteFinder finder, IList<IRouteSpecification> specifications)
        {
            foreach (IRouteSpecification spec in specifications)
            {
                yield return finder.FindRoutes(spec).Count();
            }
        }

        /// <summary>
        /// Finds the distances of the first conforming routes.
        /// </summary>
        /// <param name="routeFinder">The route finder.</param>
        /// <param name="specifications">The specifications.</param>
        /// <returns>The operations results</returns>
        public static IEnumerable<int> FindFirstConformingRouteDistance(IRouteFinder routeFinder, IList<IRouteSpecification> specifications)
        {
            foreach (IRouteSpecification spec in specifications)
            {
                IRoute route = routeFinder.FindFirstSatisfyingRoute(spec);

                if (route != default(IRoute))
                {
                    yield return route.Distance;
                }
                else
                {
                    yield return NO_SUCH_ROUTE;
                }
            }
        }

        /// <summary>
        /// Writes the operation results.
        /// </summary>
        /// <param name="results">The operations results.</param>
        public static void WriteOperationsResults(IEnumerable<int> results)
        {
            foreach (int result in results)
            {
                WriteMessage(result);
            }
        }

        /// <summary>
        /// Writes the message to the console.
        /// </summary>
        /// <param name="result">The operation result.</param>
        public static void WriteMessage(int result)
        {
            Console.WriteLine(OUTPUT_MESSAGE_FORMAT, outputCount++, result == NO_SUCH_ROUTE ? "NO SUCH ROUTE" : result.ToString());
        }
    }    
}
