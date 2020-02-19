using Aplicacao;
using Aplicacao.Mappers;
using Aplicacao.ViewModels;
using Entidade;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using Test.Start;

namespace Test.Aplicacao
{
    [TestClass]
    public class TabelaPrecoTests : BaseTests
    {
        private readonly ITabelaPrecoAvulsoAplicacao _tabelaPrecoAvulsoAplicacao;
        private readonly ITabelaPrecoSoftparkAplicacao _tabelaPrecoSoftparkAplicacao;
        private readonly IEstacionamentoSoftparkAplicacao _estacionamentoSoftparkAplicacao;

        private const int id = 999;

        public TabelaPrecoTests()
        {
            _tabelaPrecoAvulsoAplicacao = SimpleInjectorInitializer.Container.GetInstance<ITabelaPrecoAvulsoAplicacao>();
            _tabelaPrecoSoftparkAplicacao = SimpleInjectorInitializer.Container.GetInstance<ITabelaPrecoSoftparkAplicacao>();
            _estacionamentoSoftparkAplicacao = SimpleInjectorInitializer.Container.GetInstance<IEstacionamentoSoftparkAplicacao>();
        }

        [TestMethod]
        [TestCategory("Cadastrar")]
        public void TabelaPreco_Cadastrar()
        {
            var tabelaPreco = MockTests.TabelaPreco(id);
            //var tabelaPreco = new TabelaPrecoSoftparkViewModel(_tabelaPrecoAvulsoAplicacao.BuscarPorId(id));

            foreach (var tabelaPrecoEstacionamento in tabelaPreco.TabelaPrecoEstacionamento)
            {
                var estacionamento = _estacionamentoSoftparkAplicacao.BuscarPorId(tabelaPrecoEstacionamento.Estacionamento.Id);
                if (estacionamento == null)
                    _estacionamentoSoftparkAplicacao.Salvar(tabelaPrecoEstacionamento.Estacionamento);
            }

            _tabelaPrecoSoftparkAplicacao.Salvar(tabelaPreco);
        }

        [TestMethod]
        [TestCategory("BuscarPorId")]
        public void TabelaPreco_BuscarPorId()
        {
            var dado = _tabelaPrecoSoftparkAplicacao.BuscarPorId(id);

            Assert.IsTrue(dado != null);
        }

        [TestMethod]
        [TestCategory("ListarTodos")]
        public void TabelaPreco_Listar()
        {
            var dados = _tabelaPrecoSoftparkAplicacao.Listar();

            Assert.IsTrue(dados.Count() > 0);
        }

        [TestMethod]
        [TestCategory("Editar")]
        public void TabelaPreco_Editar()
        {
            var dado = _tabelaPrecoSoftparkAplicacao.BuscarPorId(id);
            dado.DizeresImpressoTicket = "editado";

            _tabelaPrecoSoftparkAplicacao.Editar(dado);

            dado = _tabelaPrecoSoftparkAplicacao.BuscarPorId(id);
            Assert.IsTrue(dado.DizeresImpressoTicket == "editado");
        }

        [TestMethod]
        [TestCategory("Deletar")]
        public void TabelaPreco_Deletar()
        {
            _tabelaPrecoSoftparkAplicacao.ExcluirPorId(id);

            var dado = _tabelaPrecoSoftparkAplicacao.BuscarPorId(id);

            Assert.IsTrue(dado == null);
        }
    }
}