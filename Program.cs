using BenchmarkDotNet.Configs;
using BenchmarkDotNet.Running;
using System;

namespace pdf_benchmarks
{
    internal class Program
    {
        static void Main(string[] args)
        {
            var config = DefaultConfig.Instance.WithOptions(ConfigOptions.DisableOptimizationsValidator);

            ////to compare work of different libs
            //var summary = BenchmarkRunner.Run<PdfComparisonBenchmark>(config);

            //to test PersitsPdf
            var summary = BenchmarkRunner.Run<PdfPersitsBenchmark>(config);

            Console.ReadLine();
        }
    }
}
