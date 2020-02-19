using Core.Extensions;
using Dominio;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Configuration;
using System.Web;

namespace Test.Dominio
{
    [Microsoft.VisualStudio.TestTools.UnitTesting.TestClass]
    public class TesteRecuperarSenha
    {
        private readonly IUsuarioServico _usuarioServico;

        public TesteRecuperarSenha()
        {
            _usuarioServico = null;
        }
        public TesteRecuperarSenha(IUsuarioServico usuarioServico)
        {
            _usuarioServico = usuarioServico;
        }

        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethod]
        public void TestGerarLink()
        {
            var chave = ConfigurationManager.AppSettings["CryptKey"];
            var original = "76|hugo.takahashi@leojarts.com.br";//"5|teste@teste.com.br";
            var originalComBase64 = original.ToBase64();
            var criptografado = Crypt.EnCrypt(originalComBase64, chave);
            var descriptografadoComBase64 = Crypt.DeCrypt(criptografado, chave);
            var descriptografado = descriptografadoComBase64.FromBase64();

            Assert.IsTrue(original.Equals(descriptografado));
        }

        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethod]
        public void TestGerarLink2()
        {
            var chave = ConfigurationManager.AppSettings["CryptKey"];
            var valor = "wbVMPTCTLq5RpXPkCNXhs7nMAgpAFWSRSmZy0QXwa33VB++Fl+kukkiDRzfRKtab";
            var descriptografadoComBase64 = Crypt.DeCrypt(valor, chave);
            var descriptografado = descriptografadoComBase64.FromBase64();

            Assert.IsTrue(!string.IsNullOrEmpty(descriptografado));
        }

        [Microsoft.VisualStudio.TestTools.UnitTesting.TestMethod]
        public void TestGerarLink3()
        {
            var chave = ConfigurationManager.AppSettings["CryptKey"];
            var valorChave = HttpUtility.UrlEncode("wbVMPTCTLq5RpXPkCNXhs7nMAgpAFWSRSmZy0QXwa33VB++Fl+kukkiDRzfRKtab");
            var valor = $"http://localhost:59662/api/v1/PedidoSelo/ReprovacaoPedido?chave={valorChave}";
            var key = HttpUtility.ParseQueryString(new Uri(valor).Query).Get("chave");
            var descriptografadoComBase64 = Crypt.DeCrypt(key, chave);
            var descriptografado = descriptografadoComBase64.FromBase64();

            Assert.IsTrue(!string.IsNullOrEmpty(descriptografado));
        }

        [TestMethod]
        public void TestEmailLink()
        {
            var chave = ConfigurationManager.AppSettings["CryptKey"];
            var valorChave = HttpUtility.UrlEncode("4uc+hJ7U88kZ8R3yasBu7kGNlA4Mwl+fxJIAlR8UPS4P+TvOkQYdgS4IG6+W8l5oZjfjaw3petU=");
            var valor = $"http://localhost:59662/api/v1/PedidoSelo/ReprovacaoPedido?chave={valorChave}";
            var key = HttpUtility.ParseQueryString(new Uri(valor).Query).Get("chave");
            var descriptografadoComBase64 = Crypt.DeCrypt(key, chave);
            var descriptografado = descriptografadoComBase64.FromBase64();

            Assert.IsTrue(!string.IsNullOrEmpty(descriptografado));
        }
        //http://grupotrevoapi.4world.com.br/api/v1/pedidoselo/aprovacaopedido?chave=
        //4uc%2bhJ7U88kZ8R3yasBu7kGNlA4Mwl%2bfxJIAlR8UPS4P%2bTvOkQYdgS4IG6%2bW8l5oZjfjaw3petU%3d
    }
}