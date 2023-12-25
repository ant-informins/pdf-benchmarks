using iText.Kernel.Colors;
using iText.Kernel.Font;
using iText.Kernel.Pdf;
using iText.Kernel.Pdf.Canvas;
using pdf_benchmarks.Common;
using System.Collections.Generic;
using System.IO;

namespace pdf_benchmarks.IText7
{
    public class IText7PdfBuilder : BasePdfBuilder
    {
        public IText7PdfBuilder() : base(PdfLibCode.IText7) { }

        public override byte[] Print(string templatePath)
        {
            byte[] blob;
            var data = GetPrintData();
            using (var stream = new MemoryStream())
            {
                using (var pdf = new PdfDocument(new PdfReader(templatePath), new PdfWriter(stream)))
                {
                    var currentFont = PdfFontFactory.CreateRegisteredFont(data.FontName);
                    var currentColor = WebColors.GetRGBColor(data.FontColor);

                    for (int i = 1; i <= pdf.GetNumberOfPages(); i++)
                    {                        
                        var page = pdf.GetPage(i);
                        var canvas = new PdfCanvas(page);                        
                        canvas.SetFontAndSize(currentFont, data.FontSize)
                            .SetColor(currentColor, true);
                        canvas.BeginText();
                        canvas.SetTextMatrix(data.X, data.Y);
                        canvas.ShowText(data.Text);
                        canvas.EndText();
                    }
                }
                blob = stream.ToArray();
            }
            return blob;
        }

        public override byte[] PrintWithMerge(string[] templatePaths)
        {
            byte[] blob;
            var items = new List<byte[]>();
            foreach (var templatePath in templatePaths)
            {
                var item = Print(templatePath);
                items.Add(item);
            }
            
            using (var stream = new MemoryStream())
            {
                using (var document = new PdfDocument(new PdfWriter(stream)))
                {
                    foreach (var item in items)
                    {
                        using (var memoryStream = new MemoryStream(item))
                        {
                            using (var itemDoc = new PdfDocument(new PdfReader(memoryStream)))
                            {
                                itemDoc.CopyPagesTo(1, itemDoc.GetNumberOfPages(), document);
                            }
                        }
                    }
                }
                blob = stream.ToArray();
            }           

            return blob;
        }
    }
}
