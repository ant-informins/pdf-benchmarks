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
            var summary = BenchmarkRunner.Run<PdfComparisonBenchmark>(config);
            
            Console.ReadLine();
        }
    }
}
