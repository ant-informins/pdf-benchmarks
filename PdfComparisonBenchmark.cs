using BenchmarkDotNet.Attributes;
using pdf_benchmarks.Common;

namespace pdf_benchmarks
{
    [MemoryDiagnoser]
    public class PdfComparisonBenchmark: PdfBenchmarkBase
    {
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
