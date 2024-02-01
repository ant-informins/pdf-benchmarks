using BenchmarkDotNet.Attributes;
using pdf_benchmarks.Common;

namespace pdf_benchmarks
{
    [MemoryDiagnoser]
    //[SimpleJob(RuntimeMoniker.Net472)]
    //[SimpleJob(RuntimeMoniker.Net48)]
    //[SimpleJob(RuntimeMoniker.Net481)]
    public class PdfPersitsBenchmark : PdfBenchmarkBase
    {
        [Benchmark]
        public void PrintMT()
        {
            TestPrintMT(PdfLibCode.PersitsPdf, 10);
        }
        
        [Benchmark]
        public void PrintWithMergeMT()
        {
            TestPrintWithMergeMT(PdfLibCode.PersitsPdf, 10);
        }
    }
}
