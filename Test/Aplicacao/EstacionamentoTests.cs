using Aplicacao;
using Aplicacao.Mappers;
using Aplicacao.ViewModels;
using Dominio;
using Entidade;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using Test.Start;
using WebServices;

namespace Test.Aplicacao
{
    [TestClass]
    public class EstacionamentoTests : BaseTests
    {
        private readonly IUnidadeServico _unidadeServico;
        private readonly IEstacionamentoSoftparkAplicacao _estacionamentoSoftparkAplicacao;

        private const int id = 1;

        public EstacionamentoTests()
        {
            _unidadeServico = SimpleInjectorInitializer.Container.GetInstance<IUnidadeServico>();
            _estacionamentoSoftparkAplicacao = SimpleInjectorInitializer.Container.GetInstance<IEstacionamentoSoftparkAplicacao>();
        }

        [TestMethod]
        [TestCategory("SalvarTodos")]
        public void Estacionamento_SalvarTodos()
        {
            var unidades = _unidadeServico.Buscar();
            var estacionamentos = unidades.Select(x => new EstacionamentoSoftparkViewModel(x)).ToList();

            _estacionamentoSoftparkAplicacao.SalvarTodos(estacionamentos);
        }

        [TestMethod]
        [TestCategory("Cadastrar")]
        public void Estacionamento_Cadastrar()
        {
            //var estacionamento = MockTests.Estacionamento(999);
            var unidade = _unidadeServico.BuscarPorId(id);
            var estacionamento = new EstacionamentoSoftparkViewModel(unidade);

            _estacionamentoSoftparkAplicacao.Salvar(estacionamento);
        }

        [TestMethod]
        [TestCategory("BuscarPorId")]
        public void Estacionamento_BuscarPorId()
        {
            var dado = _estacionamentoSoftparkAplicacao.BuscarPorId(id);

            Assert.IsTrue(dado != null);
        }

        [TestMethod]
        [TestCategory("ListarTodos")]
        public void Estacionamento_ListarTodos()
        {
            var dados = _estacionamentoSoftparkAplicacao.Listar();

            Assert.IsTrue(dados.Count() > 0);
        }

        [TestMethod]
        [TestCategory("Editar")]
        public void Estacionamento_Editar()
        {
            var token = TokenWebService.BuscarToken();
            var dado = _estacionamentoSoftparkAplicacao.BuscarPorId(id);
            dado.Nome = "editado";

            _estacionamentoSoftparkAplicacao.Editar(dado);

            dado = _estacionamentoSoftparkAplicacao.BuscarPorId(id);
            Assert.IsTrue(dado.Nome == "editado");
        }

        [TestMethod]
        [TestCategory("Deletar")]
        public void Estacionamento_Deletar()
        {
            _estacionamentoSoftparkAplicacao.ExcluirPorId(id);

            var dado = _estacionamentoSoftparkAplicacao.BuscarPorId(id);

            Assert.IsTrue(dado == null);
        }
    }
}