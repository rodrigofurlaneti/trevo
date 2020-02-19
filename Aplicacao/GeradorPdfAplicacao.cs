using Dominio;
using SelectPdf;
using System;
using System.Collections.Generic;
using System.IO;

namespace Aplicacao
{
    public interface IGeradorPdfAplicacao
    {
        Stream GerarPdfStream(string html);
        byte[] GerarPdfByte(string html);
        byte[] GerarPdfCNAB(List<KeyValuePair<int, string>> htmlIndexComComprovantes, byte[] pdfBoletos);
    }

    public class GeradorPdfAplicacao : IGeradorPdfAplicacao
    {
        private readonly IGeradorPdfServico _geradorPdfServico;

        public GeradorPdfAplicacao(IGeradorPdfServico geradorPdfServico)
        {
            _geradorPdfServico = geradorPdfServico;
        }

        public Stream GerarPdfStream(string html)
        {
            return _geradorPdfServico.GerarPdfStream(html);
        }

        public byte[] GerarPdfByte(string html)
        {
            return _geradorPdfServico.GerarPdfByte(html);
        }

        public byte[] GerarPdfCNAB(List<KeyValuePair<int, string>> htmlIndexComComprovantes, byte[] pdfBoletos)
        {
            return _geradorPdfServico.GerarPdfCNAB(htmlIndexComComprovantes, pdfBoletos);
        }
    }
}