using System;
using System.IO;
using System.Collections.Generic;
using SelectPdf;
using System.Linq;

namespace Dominio
{
    public interface IGeradorPdfServico
    {
        Stream GerarPdfStream(string html);
        byte[] GerarPdfByte(string html);
        byte[] GerarPdfCNAB(List<KeyValuePair<int, string>> htmlIndexComComprovantes, byte[] pdfBoletos);
    }

    public class GeradorPdfServico : IGeradorPdfServico
    {
        public Stream GerarPdfStream(string html)
        {
            return new MemoryStream(GerarPdfByte(html));
        }

        public byte[] GerarPdfByte(string html)
        {
            var pageSize = (PdfPageSize)Enum.Parse(typeof(PdfPageSize), "A4", true);
            var pdfOrientation = (PdfPageOrientation)Enum.Parse(typeof(PdfPageOrientation), "Portrait", true);

            var converter = new HtmlToPdf();
            converter.Options.PdfPageSize = pageSize;
            converter.Options.PdfPageOrientation = pdfOrientation;
            converter.Options.AutoFitHeight = HtmlToPdfPageFitMode.AutoFit;
            converter.Options.AutoFitWidth = HtmlToPdfPageFitMode.AutoFit;

            var doc = converter.ConvertHtmlString(html);
            var pdf = doc.Save();
            doc.Close();

            return pdf;
        }

        /// <summary>
        /// Gerar PDF do CNAB
        /// </summary>
        /// <param name="htmlIndexComComprovantes"> (Int) - Index para Inserir os dados de Comprovantes, (String) - Comprovantes </param>
        /// <param name="pdfBoletos"></param>
        /// <returns></returns>
        public byte[] GerarPdfCNAB(List<KeyValuePair<int, string>> htmlIndexComComprovantes, byte[] pdfBoletos)
        {
            var pdfBoletosDoc = new PdfDocument(new MemoryStream(pdfBoletos));
            var pdfNew = new PdfDocument();

            for (int i = 0; i < pdfBoletosDoc.Pages.Count; i++)
            {
                if (htmlIndexComComprovantes.Any(x => x.Key == i))
                {
                    var doc = new PdfDocument(new MemoryStream(GerarPdfByte(htmlIndexComComprovantes.FirstOrDefault(x => x.Key == i).Value)));
                    foreach (PdfPage pageItem in doc.Pages)
                    {
                        pdfNew.Pages.Add(pageItem);
                    }
                }
                pdfNew.Pages.Add(pdfBoletosDoc.Pages[i]);
            }

            var pdfBytesFinal = pdfNew.Save();
            pdfNew.Close();

            return pdfBytesFinal;
        }
    }
}