using BenchmarkDotNet.Attributes;
using pdf_benchmarks.Common;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace pdf_benchmarks
{
    [MemoryDiagnoser]
    public class PdfComparisonBenchmark
    {
        private readonly string _testDataPath;

        private readonly string _rootPath;

        public PdfComparisonBenchmark()
        {
            _rootPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data");
            if (!Directory.Exists(_rootPath))
            {
                Directory.CreateDirectory(_rootPath);
            }
            var projectDirectory = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.FullName;
            _testDataPath = Path.Combine(projectDirectory, "Templates");
        }
        
        public void TestPrint(PdfLibCode libCode, int index = 0, bool autoopen = false)
        {            
            var templatePath = Path.Combine(_testDataPath, "test1.pdf");
            var resultPath = Path.Combine(_rootPath, $"{libCode}-{index}-{Guid.NewGuid()}.pdf");

            var pdfBuilder = PdfBuilderFactory.Create(libCode);
            var blob = pdfBuilder.Print(templatePath);
            File.WriteAllBytes(resultPath, blob);
            if (autoopen)
            {
                Process.Start(resultPath);
            }
        }

        public void TestPrintMT(PdfLibCode libCode, int maxDegreeOfParallelism, bool autoopen = false)
        {
            var parallelOptions = new ParallelOptions { MaxDegreeOfParallelism = maxDegreeOfParallelism };
            var nums = Enumerable.Range(0, maxDegreeOfParallelism).ToList();
            Parallel.ForEach(nums, parallelOptions, _ => {
                TestPrint(libCode, _, autoopen);
            });
        }
       
        public void TestPrintWithMerge(PdfLibCode libCode, int index = 0, bool autoopen = false)
        {
            var templatePaths = new string[] {
                Path.Combine(_testDataPath, "test1.pdf"),
                Path.Combine(_testDataPath, "test1.pdf"),
                Path.Combine(_testDataPath, "test1.pdf"),
            };
            var resultPath = Path.Combine(_rootPath, $"{libCode}-{index}-{Guid.NewGuid()}.pdf");

            var pdfBuilder = PdfBuilderFactory.Create(libCode);
            var blob = pdfBuilder.PrintWithMerge(templatePaths);
            File.WriteAllBytes(resultPath, blob);
            if (autoopen)
            {
                Process.Start(resultPath);
            }
        }

        public void TestPrintWithMergeMT(PdfLibCode libCode, int maxDegreeOfParallelism, bool autoopen = false)
        {
            var parallelOptions = new ParallelOptions { MaxDegreeOfParallelism = maxDegreeOfParallelism };
            var nums = Enumerable.Range(0, maxDegreeOfParallelism).ToList();
            Parallel.ForEach(nums, parallelOptions, _ => {
                TestPrintWithMerge(libCode, _, autoopen);
            });
        }


        [Benchmark]
        [MaxIterationCount(20)]
        [Arguments(PdfLibCode.PersitsPdf)]
        [Arguments(PdfLibCode.IText7)]
        public void Print(PdfLibCode libCode)
        {
            TestPrint(libCode);
        }
        
        [Benchmark]
        [MaxIterationCount(20)]
        [Arguments(PdfLibCode.PersitsPdf)]
        [Arguments(PdfLibCode.IText7)]
        public void PrintMT(PdfLibCode libCode)
        {
            TestPrintMT(libCode, 10);
        }
               
        [Benchmark]
        [MaxIterationCount(20)]
        [Arguments(PdfLibCode.PersitsPdf)]
        [Arguments(PdfLibCode.IText7)]
        public void PrintWithMerge(PdfLibCode libCode)
        {
            TestPrintWithMerge(libCode);
        }
               
        [Benchmark]        
        [MaxIterationCount(20)]
        [Arguments(PdfLibCode.PersitsPdf)]
        [Arguments(PdfLibCode.IText7)]
        public void PrintWithMergeMT(PdfLibCode libCode)
        {
            TestPrintWithMergeMT(libCode, 10);
        }
    }
}
