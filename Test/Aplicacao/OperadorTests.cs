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
    public class OperadorTests : BaseTests
    {
        private readonly IUsuarioAplicacao _usuarioAplicacao;
        private readonly IOperadorSoftparkAplicacao _operadorSoftparkAplicacao;

        private const int id = 7;

        public OperadorTests()
        {
            _usuarioAplicacao = SimpleInjectorInitializer.Container.GetInstance<IUsuarioAplicacao>();
            _operadorSoftparkAplicacao = SimpleInjectorInitializer.Container.GetInstance<IOperadorSoftparkAplicacao>();
        }

        [TestMethod]
        [TestCategory("Cadastrar")]
        public void Operador_Cadastrar()
        {
            var usuario = _usuarioAplicacao.BuscarPorId(id);

            var operador = new OperadorSoftparkViewModel(usuario);
            _operadorSoftparkAplicacao.Salvar(operador);
        }

        [TestMethod]
        [TestCategory("SalvarTodos")]
        public void Operador_SalvarTodosComAcessoAoPDV()
        {
            var usuarios = _usuarioAplicacao.Buscar();

            foreach (var usuario in usuarios)
            {
                if (!usuario.TemAcessoAoPDV)
                    Assert.Fail("Usuário não tem acesso ao PDV");

                var operador = new OperadorSoftparkViewModel(usuario);

                _operadorSoftparkAplicacao.Salvar(operador);
            }
        }

        [TestMethod]
        [TestCategory("BuscarPorId")]
        public void Operador_BuscarPorId()
        {
            var dado = _operadorSoftparkAplicacao.BuscarPorId(id);

            Assert.IsTrue(dado != null);
        }

        [TestMethod]
        [TestCategory("ListarTodos")]
        public void Operador_ListarTodos()
        {
            var dados = _operadorSoftparkAplicacao.Listar();

            Assert.IsTrue(dados.Count() > 0);
        }

        [TestMethod]
        [TestCategory("Editar")]
        public void Operador_Editar()
        {
            var dado = _operadorSoftparkAplicacao.BuscarPorId(id);
            dado.Matricula = "editado";

            _operadorSoftparkAplicacao.Editar(dado);

            dado = _operadorSoftparkAplicacao.BuscarPorId(id);
            Assert.IsTrue(dado.Matricula == "editado");
        }

        [TestMethod]
        [TestCategory("Deletar")]
        public void Operador_Deletar()
        {
            _operadorSoftparkAplicacao.ExcluirPorId(id);

            var dado = _operadorSoftparkAplicacao.BuscarPorId(id);

            Assert.IsTrue(dado == null);
        }
    }
}