using pdf_benchmarks.IText7;
using pdf_benchmarks.PersitsPdf;

namespace pdf_benchmarks.Common
{
    public static class PdfBuilderFactory
    {
        public static BasePdfBuilder Create(PdfLibCode code)
        {
            return code switch
            {
                PdfLibCode.PersitsPdf => new PersitsPdfBuilder(),
                PdfLibCode.IText7 => new IText7PdfBuilder(),
                _ => null
            };
        }
    }
}
