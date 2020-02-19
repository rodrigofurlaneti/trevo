//using SelectPdf;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using iTextSharp.text;
using iTextSharp.text.pdf;
using iTextSharp.tool.xml;

namespace Core
{
    public static class Tools
    {
        public static string GenerateRandomValue()
        {
            var guid = Guid.NewGuid().ToString().Replace("-", "");
            var random = new Random();
            var length = random.Next(6, 8);
            var password = "";
            for (var i = 0; i <= length; i++)
                password += guid.Substring(random.Next(1, guid.Length), 1);
            //
            return password;
        }

        public static string GenerateRandomCode(int length)
        {
            var code = "";
            var random = new Random();
            for (var i = 0; i < 8; i++)
            {
                var randomNumber = random.Next(0, 9);
                code += randomNumber.ToString();
            }
            //
            return code;
        }

        public static string GetImageFromBase64(byte[] imgBytes)
        {
            return $"data:image/jpg;base64,{Convert.ToBase64String(imgBytes)}";
        }

        public static Stream GerarPdfFromHtmlToStream(string htmlContent)
        {
            return new MemoryStream(GerarPdfFromHtmlToByte(htmlContent));
        }

        public static byte[] GerarPdfFromHtmlToByte(string htmlContent)
        {
            return new NReco.PdfGenerator.HtmlToPdfConverter().GeneratePdf(htmlContent);
        }


        //public static Stream GerarPdfStream(string html)
        //{
        //    return new MemoryStream(GerarPdfByte(html));
        //}

        //public static byte[] GerarPdfByte(string html)
        //{
        //    var pageSize = (PdfPageSize)Enum.Parse(typeof(PdfPageSize), "A4", true);
        //    var pdfOrientation = (PdfPageOrientation)Enum.Parse(typeof(PdfPageOrientation), "Portrait", true);

        //    var converter = new HtmlToPdf();
        //    converter.Options.PdfPageSize = pageSize;
        //    converter.Options.PdfPageOrientation = pdfOrientation;
        //    converter.Options.AutoFitHeight = HtmlToPdfPageFitMode.AutoFit;
        //    converter.Options.AutoFitWidth = HtmlToPdfPageFitMode.AutoFit;

        //    var doc = converter.ConvertHtmlString(html);
        //    var pdf = doc.Save();
        //    doc.Close();

        //    return pdf;
        //}

        /// <summary>
        /// Gerar PDF do CNAB
        /// </summary>
        /// <param name="htmlIndexComComprovantes"> (Int) - Index para Inserir os dados de Comprovantes, (String) - Comprovantes </param>
        /// <param name="pdfBoletos"></param>
        /// <returns></returns>
        public static byte[] GerarPdfCNAB(List<KeyValuePair<int, string>> htmlIndexComComprovantes, byte[] pdfBoletos)
        {
            byte[] all;

            using (MemoryStream ms = new MemoryStream())
            {
                Document doc = new Document();

                PdfWriter writer = PdfWriter.GetInstance(doc, ms);

                doc.SetPageSize(PageSize.A4);
                doc.Open();
                PdfContentByte cb = writer.DirectContent;
                PdfImportedPage page;

                PdfReader reader;
                reader = new PdfReader(pdfBoletos);
                int pages = reader.NumberOfPages;
                
                for (int i = 1; i <= pages; i++)
                {
                    if (htmlIndexComComprovantes.Any(x => x.Key == i))
                    {
                        using (var memoryStream = new MemoryStream())
                        {
                            byte[] pdf;
                            var document = new Document(PageSize.A4, 20, 20, 50, 50);
                            var writerHtml = PdfWriter.GetInstance(document, memoryStream);
                            document.Open();

                            using (var htmlMemoryStream = new MemoryStream(System.Text.Encoding.UTF8.GetBytes(htmlIndexComComprovantes.FirstOrDefault(x => x.Key == i).Value)))
                            {
                                XMLWorkerHelper.GetInstance().ParseXHtml(writerHtml, document, htmlMemoryStream, new MemoryStream());
                            }
                            
                            document.Close();

                            pdf = memoryStream.ToArray();

                            var readerHtml = new PdfReader(pdf);
                            int pagesHtml = readerHtml.NumberOfPages;

                            for (int k = 1; k <= pagesHtml; k++)
                            {
                                PdfImportedPage pageHtml;
                                doc.SetPageSize(PageSize.A4);
                                doc.NewPage();
                                pageHtml = writer.GetImportedPage(readerHtml, k);
                                cb.AddTemplate(pageHtml, 0, 0);
                            }
                        }
                    }

                    doc.SetPageSize(PageSize.A4);
                    doc.NewPage();
                    page = writer.GetImportedPage(reader, i);
                    cb.AddTemplate(page, 0, 0);
                }

                doc.Close();
                all = ms.GetBuffer();
                ms.Flush();
                ms.Dispose();
            }

            return all;
        }
    }
}
