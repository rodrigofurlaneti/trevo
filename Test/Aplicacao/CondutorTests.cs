using Aplicacao;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Linq;
using Test.Start;

namespace Test.Aplicacao
{
    [TestClass]
    public class CondutorTests : BaseTests
    {
        private readonly ICondutorSoftparkAplicacao _condutorSoftparkAplicacao;
        private readonly IEstacionamentoSoftparkAplicacao _estacionamentoSoftparkAplicacao;
        private readonly IClienteAplicacao _clienteAplicacao;

        private const int id = 49;

        public CondutorTests()
        {
            _condutorSoftparkAplicacao = SimpleInjectorInitializer.Container.GetInstance<ICondutorSoftparkAplicacao>();
            _estacionamentoSoftparkAplicacao = SimpleInjectorInitializer.Container.GetInstance<IEstacionamentoSoftparkAplicacao>();
            _clienteAplicacao = SimpleInjectorInitializer.Container.GetInstance<IClienteAplicacao>();
        }
        
        [TestMethod]
        [TestCategory("Cadastrar")]
        public void Condutor_Cadastrar()
        {
            var cliente = _clienteAplicacao.BuscarPorId(id);
            var condutor = _clienteAplicacao.ConverterClienteParaCondutor(cliente);

            _condutorSoftparkAplicacao.Salvar(condutor);
        }

        [TestMethod]
        [TestCategory("BuscarPorId")]
        public void Condutor_BuscarPorId()
        {
            var dado = _condutorSoftparkAplicacao.BuscarPorId(id);

            Assert.IsTrue(dado != null);
        }

        [TestMethod]
        [TestCategory("ListarTodos")]
        public void Condutor_Listar()
        {
            var dados = _condutorSoftparkAplicacao.Listar();

            Assert.IsTrue(dados.Count() > 0);
        }

        [TestMethod]
        [TestCategory("Editar")]
        public void Condutor_Editar()
        {
            var dado = _condutorSoftparkAplicacao.BuscarPorId(id);
            dado.Nome = "editado";

            _condutorSoftparkAplicacao.Editar(dado);

            dado = _condutorSoftparkAplicacao.BuscarPorId(id);
            Assert.IsTrue(dado.Nome == "editado");
        }

        [TestMethod]
        [TestCategory("Deletar")]
        public void Condutor_Deletar()
        {
            _condutorSoftparkAplicacao.ExcluirPorId(id);

            var dado = _condutorSoftparkAplicacao.BuscarPorId(id);

            Assert.IsTrue(dado == null);
        }
    }
}