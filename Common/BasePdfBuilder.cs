using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace pdf_benchmarks.Common
{
    public abstract class BasePdfBuilder
    {
        public BasePdfBuilder(PdfLibCode libCode)
        {
            LibCode = libCode;
        }

        public PdfLibCode LibCode { get; private set; }
        
        public abstract byte[] Print(string templatePath);

        public abstract byte[] PrintWithMerge(string[] templatePaths);

        protected PrintData GetPrintData()
        {
            return new PrintData
            {
                X = 300,
                Y = 600,
                Text = "Test",
                FontName = "Times-Roman",
                FontColor = "red",
                FontSize = 14,
            };
        }

        protected class PrintData
        {
            public float X { get; set; }
            public float Y { get; set; }
            public string Text { get; set; }
            public string FontName { get; set; }
            public int FontSize { get; set; }
            public string FontColor { get; set; }
        }
    }
}
