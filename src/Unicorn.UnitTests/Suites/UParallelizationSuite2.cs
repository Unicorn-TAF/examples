using System.Threading;
using Unicorn.Taf.Core.Testing;
using Unicorn.Taf.Core.Testing.Attributes;

namespace Unicorn.UnitTests.Suites
{
    [Suite("Suite that tests parallelization by suite 2")]
    [Tag("parallelizaton")]
    public class UParallelizationSuite2 : TestSuite
    {
        public static string Output { get; set; }

        [BeforeSuite]
        public void BeforeSuite()
        {
            //
        }

        public string GetOutput()
        {
            return Output;
        }

        [Test("ParallelizationBySuite2-1")]
        [Category("category2")]
        public void Test1()
        {
            Thread.Sleep(System.TimeSpan.FromSeconds(1));

            Output += "Test1>";
        }

        [Test("ParallelizationBySuite2-2")]
        [Category("category1")]
        public void Test2()
        {
            Thread.Sleep(System.TimeSpan.FromSeconds(1));

            Output += "Test2>";
        }

        [Test("ParallelizationBySuite2-3")]
        [Category("category2")]
        public void Test3()
        {
            Thread.Sleep(System.TimeSpan.FromSeconds(1));

            Output += "Test2>";
        }
    }
}
