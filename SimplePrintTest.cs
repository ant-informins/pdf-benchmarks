using BenchmarkDotNet.Attributes;
using Persits.PDF;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pdf_benchmarks
{
    [MemoryDiagnoser]
    public class SimplePrintTest
    {
        private const string ASP_PDF_REG_KEY = "";
        private readonly string _rootPath;
        private readonly string _testDataPath;

        public SimplePrintTest()
        {
            _rootPath = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "data");
            if (!Directory.Exists(_rootPath))
            {
                Directory.CreateDirectory(_rootPath);
            }
            var projectDirectory = Directory.GetParent(AppDomain.CurrentDomain.BaseDirectory).Parent.Parent.FullName;
            _testDataPath = Path.Combine(projectDirectory, "Templates");
        }


        [Benchmark]
        public void Print()
        {
            int maxDegreeOfParallelism = 10;
            var templatePath = Path.Combine(_testDataPath, "test1.pdf");

            var parallelOptions = new ParallelOptions { MaxDegreeOfParallelism = maxDegreeOfParallelism };
            var nums = Enumerable.Range(0, maxDegreeOfParallelism).ToList();
            Parallel.ForEach(nums, parallelOptions, _ => {
                var manager = new PdfManager();
                manager.RegKey = ASP_PDF_REG_KEY;

                var blob = Print(manager, templatePath);
                var resultPath = Path.Combine(_rootPath, $"{Guid.NewGuid()}.pdf");
                File.WriteAllBytes(resultPath, blob);
            });
        }

        [Benchmark]
        public void PrintWithMerge()
        {
            int maxDegreeOfParallelism = 10;
            var templatePaths = new string[] {
                Path.Combine(_testDataPath, "test1.pdf"),
                Path.Combine(_testDataPath, "test1.pdf"),
            };

            var parallelOptions = new ParallelOptions { MaxDegreeOfParallelism = maxDegreeOfParallelism };
            var nums = Enumerable.Range(0, maxDegreeOfParallelism).ToList();
            Parallel.ForEach(nums, parallelOptions, _ => {
                var manager = new PdfManager();
                manager.RegKey = ASP_PDF_REG_KEY;

                var blob = PrintWithMerge(manager, templatePaths);
                var resultPath = Path.Combine(_rootPath, $"{Guid.NewGuid()}.pdf");
                File.WriteAllBytes(resultPath, blob);
            });
        }

        private byte[] Print(PdfManager manager, string templatePath)
        {
            byte[] blob;
            using (var document = manager.OpenDocument(templatePath))
            {
                for (int i = 1; i <= document.Pages.Count; i++)
                {
                    var page = document.Pages[i];
                    var canvas = page.Canvas;

                    page.ResetCoordinates();

                    var pdfFont = document.Fonts["Times-Roman"];
                    var pdfParam = manager.CreateParam();
                    pdfParam["size"] = 14;
                    pdfParam.Add($"color=red");
                    pdfParam["x"] = 300;
                    pdfParam["y"] = 600;
                    canvas.DrawText($"TEST_{i}", pdfParam, pdfFont);
                }

                blob = document.SaveToMemory();
            }
            return blob;
        }
        private byte[] PrintWithMerge(PdfManager manager, string[] templatePaths)
        {
            byte[] blob;
            var items = new List<byte[]>();
            foreach (var templatePath in templatePaths)
            {
                var item = Print(manager, templatePath);
                items.Add(item);
            }

            using (var document = manager.CreateDocument())
            {
                foreach (var item in items)
                {
                    using (var doc = manager.OpenDocument(item))
                    {
                        document.AppendDocument(doc);
                    }
                }
                blob = document.SaveToMemory();
            }
            return blob;
        }
    }
}
