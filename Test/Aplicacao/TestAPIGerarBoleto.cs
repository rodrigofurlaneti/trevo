using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.IO;
using System.Net;
using System.Text;

namespace Test.Aplicacao
{
    [TestClass]
    public class TestAPIGerarBoleto
    {
        [TestMethod]
        public void TestGerarBoleto()
        {
            var baseAddress = "http://grupotrevoapi.4world.com.br/api/v1/PedidoSelo/GerarLancamentoCobranca";

            var http = (HttpWebRequest)WebRequest.Create(new Uri(baseAddress));
            http.Accept = "application/json";
            http.ContentType = "application/json";
            http.Method = "POST";

            string parsedContent = "{ \"Id\": 91 }";
            ASCIIEncoding encoding = new ASCIIEncoding();
            Byte[] bytes = encoding.GetBytes(parsedContent);

            Stream newStream = http.GetRequestStream();
            newStream.Write(bytes, 0, bytes.Length);
            newStream.Close();

            var response = http.GetResponse();

            var stream = response.GetResponseStream();
            var sr = new StreamReader(stream);
            var content = sr.ReadToEnd();
        }
    }
}