using Dominio;
using Entidade.Uteis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using Test.Start;

namespace Test.Dominio
{
    [TestClass]
    public class TestePedidoSelo : BaseTests
    {
        private readonly IPedidoSeloServico _pedidoSeloServico;
        private readonly INotificacaoServico _notificacaoServico;

        public TestePedidoSelo()
        {
            _pedidoSeloServico = SimpleInjectorInitializer.Container.GetInstance<IPedidoSeloServico>();
            _notificacaoServico = SimpleInjectorInitializer.Container.GetInstance<INotificacaoServico>();
        }
        
        [TestMethod]
        public void TestNotificacaoWorkflow()
        {
            var idNotificacaoLista = new List<int> { 244 }; //236, 237, 239, 240, 241, 

            foreach (var idNot in idNotificacaoLista)
            {
                var notificacao = _notificacaoServico.BuscarPorId(idNot);
                _notificacaoServico.AtualizarStatus(idNot, notificacao.TipoNotificacao.Entidade, 1, AcaoNotificacao.Aprovado, null);
            }
        }

        [TestMethod]
        public void TestCalculaValorPedidoSelo()
        {
            //Valor no boleto está como 750, porém deve ficar com 600! PedidoSeloId - 464
            var pedidoSelo = _pedidoSeloServico.BuscarPorId(464);

            var valorFinal = _pedidoSeloServico.CalculaValorLancamentoCobranca(pedidoSelo);

            Assert.AreEqual(600, valorFinal);
        }
    }
}