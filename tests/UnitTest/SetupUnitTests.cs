using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;

namespace UnitTest
{
    [TestClass]
    public class SetupUnitTests
    {
        // A class that contains MSTest unit tests. (Required)
        [TestClass]
        public class Initialize
        {
            [AssemblyInitialize]
            public static void AssemblyInit(TestContext context)
            {
                // Executes once before the test run. (Optional)
                Console.WriteLine("AssemblyInit");
            }
        }

        [TestClass]
        public class DeInitialize
        {
            [AssemblyCleanup]
            public static void AssemblyCleanup()
            {
                // Executes once after the test run. (Optional)
                Console.WriteLine("AssemblyCleanup");
            }
        }
    }
}
