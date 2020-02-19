using Aplicacao;
using Aplicacao.Mappers;
using Aplicacao.ViewModels;
using Entidade;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using Test.Start;
using WebServices;

namespace Test.Aplicacao
{
    [TestClass]
    public class TabelaPrecoMensalTests : BaseTests
    {
        private readonly ITabelaPrecoMensalistaAplicacao _tabelaPrecoMensalistaAplicacao;
        private readonly ITabelaPrecoMensalSoftparkAplicacao _tabelaPrecoMensalSoftparkAplicacao;

        private const int id = 40;

        public TabelaPrecoMensalTests()
        {
            _tabelaPrecoMensalistaAplicacao = SimpleInjectorInitializer.Container.GetInstance<ITabelaPrecoMensalistaAplicacao>();
            _tabelaPrecoMensalSoftparkAplicacao = SimpleInjectorInitializer.Container.GetInstance<ITabelaPrecoMensalSoftparkAplicacao>();
        }

        [TestMethod]
        [TestCategory("Cadastrar")]
        public void TabelaPrecoMensal_Cadastrar()
        {
            //var tabelaPrecoMensalista = MockTests.TabelaPrecoMensal(id);
            var tabelaPrecoMensalista = _tabelaPrecoMensalistaAplicacao.BuscarPorId(id);

            var tabelaPrecoMensal = new TabelaPrecoMensalSoftparkViewModel(tabelaPrecoMensalista);

            _tabelaPrecoMensalSoftparkAplicacao.Salvar(tabelaPrecoMensal);
        }

        [TestMethod]
        [TestCategory("BuscarPorId")]
        public void TabelaPrecoMensal_BuscarPorId()
        {
            var dado = _tabelaPrecoMensalSoftparkAplicacao.BuscarPorId(id);

            Assert.IsTrue(dado != null);
        }

        [TestMethod]
        [TestCategory("ListarTodos")]
        public void TabelaPrecoMensal_ListarTodos()
        {
            var dados = _tabelaPrecoMensalSoftparkAplicacao.Listar();

            Assert.IsTrue(dados.Count() > 0);
        }

        [TestMethod]
        [TestCategory("Editar")]
        public void TabelaPrecoMensal_Editar()
        {
            var dado = _tabelaPrecoMensalSoftparkAplicacao.BuscarPorId(id);
            dado.Periodo = "editado";

            _tabelaPrecoMensalSoftparkAplicacao.Editar(dado);

            dado = _tabelaPrecoMensalSoftparkAplicacao.BuscarPorId(id);
            Assert.IsTrue(dado.Periodo == "editado");
        }

        [TestMethod]
        [TestCategory("Deletar")]
        public void TabelaPrecoMensal_Deletar()
        {
            _tabelaPrecoMensalSoftparkAplicacao.ExcluirPorId(id);

            var dado = _tabelaPrecoMensalSoftparkAplicacao.BuscarPorId(id);

            Assert.IsTrue(dado == null);
        }
    }
}