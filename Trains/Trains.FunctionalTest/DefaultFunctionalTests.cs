using NUnit.Framework;
using System;
using System.IO;
using System.Text;

namespace Trains.FunctionalTest
{
    [TestFixture]
    public class DefaultFunctionalTests
    {
        /// <summary>
        /// Contains the current testoutput
        /// </summary>
        private StringBuilder testOutput;

        /// <summary>
        /// Setups the test.
        /// </summary>
        [SetUp]
        public void SetupTest()
        {
            this.testOutput = new StringBuilder();
            Console.SetOut(new StringWriter(this.testOutput));
        }

        /// <summary>
        /// Tears down the test.
        /// </summary>
        [TearDown]
        public void TeardownTest()
        {
            Console.SetOut(Console.Out);
            this.testOutput.Clear();
            this.testOutput = null;
        }

        /// <summary>
        /// Tests the route calculator results.
        /// </summary>
        /// <param name="fileName">The filename with the test data.</param>
        /// <param name="expectedOutput">The expected output of the target.</param>
        [Test]
        [TestCase(null, "Please specify a filename argument.")]
        [TestCase("test_data/bad_data.txt", "An error ocurred while trying to read the specified file: test_data/bad_data.txt")]
        [TestCase("unexistant_file", "The specified file: unexistant_file does not exist.")]
        [TestCase("test_data/default_data.txt", "Output #1: 9")]
        [TestCase("test_data/default_data.txt", "Output #2: 5")]
        [TestCase("test_data/default_data.txt", "Output #3: 13")]
        [TestCase("test_data/default_data.txt", "Output #4: 22")]
        [TestCase("test_data/default_data.txt", "Output #5: NO SUCH ROUTE")]
        [TestCase("test_data/default_data.txt", "Output #6: 2")]
        [TestCase("test_data/default_data.txt", "Output #7: 3")]
        [TestCase("test_data/default_data.txt", "Output #8: 9")]
        [TestCase("test_data/default_data.txt", "Output #9: 9")]
        [TestCase("test_data/default_data.txt", "Output #10: 7")]
        [TestCase("test_data/default_data.txt", " ========== ")]
        public void TestRouteCalculatorResults(string fileName, string expectedOutput)
        {
            // Arrange            
            string output = string.Empty;
            string[] argument = fileName == null ? new string[] { } : new string[] { fileName };

            // Act
            Trains.Program.Main(argument);
            output = this.testOutput.ToString();

            // Assert
            StringAssert.Contains(expectedOutput, output);
        }
    }
}
