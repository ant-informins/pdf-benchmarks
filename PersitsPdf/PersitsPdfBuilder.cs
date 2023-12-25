using pdf_benchmarks.Common;
using Persits.PDF;
using System.Collections.Generic;

namespace pdf_benchmarks.PersitsPdf
{
    public class PersitsPdfBuilder : BasePdfBuilder
    {        
        private const string ASP_PDF_REG_KEY = "";

        public PersitsPdfBuilder() : base(PdfLibCode.PersitsPdf) { }

        public override byte[] Print(string templatePath)
        {
            var manager = new PdfManager();
            manager.RegKey = ASP_PDF_REG_KEY;
            return Print(manager, templatePath);
        }

        public override byte[] PrintWithMerge(string[] templatePaths)
        {
            byte[] blob;
            var manager = new PdfManager();
            manager.RegKey = ASP_PDF_REG_KEY;

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
              
        private byte[] Print(PdfManager manager, string templatePath)
        {
            byte[] blob;
            var data = GetPrintData();
            using (var document = manager.OpenDocument(templatePath))
            {
                var currentFont = document.Fonts[data.FontName];

                for (int i = 1; i <= document.Pages.Count; i++)
                {
                    var page = document.Pages[i];
                    var canvas = page.Canvas;

                    page.ResetCoordinates();
                    
                    var pdfParam = manager.CreateParam();
                    pdfParam["size"] = data.FontSize;
                    pdfParam.Add($"color={data.FontColor}");
                    pdfParam["x"] = data.X;
                    pdfParam["y"] = data.Y;
                    canvas.DrawText(data.Text, pdfParam, currentFont);
                }
                blob = document.SaveToMemory();
            }
            return blob;
        }
    }
}
