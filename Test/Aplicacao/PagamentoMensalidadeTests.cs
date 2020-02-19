//using Aplicacao;
//using Aplicacao.Mappers;
//using Aplicacao.ViewModels;
//using Entidade;
//using Microsoft.VisualStudio.TestTools.UnitTesting;
//using System;
//using System.Linq;
//using Test.Start;
//using WebServices;

//namespace Test.Aplicacao
//{
//    [TestClass]
//    public class PagamentoMensalidadeTests : BaseTests
//    {
//        private readonly IUnidadeAplicacao _unidadeAplicacao;

//        private const int id = 999;

//        public PagamentoMensalidadeTests()
//        {
//            _unidadeAplicacao = SimpleInjectorInitializer.Container.GetInstance<IUnidadeAplicacao>();
//        }

//        [TestMethod]
//        [TestCategory("Cadastrar")]
//        public void PagamentoMensalidade_Cadastrar()
//        {
//            var token = TokenWebService.BuscarToken();
//            var unidade = _unidadeAplicacao.PrimeiroPor(x => true);

//            var pagamentoMensalidade = MockTests.PagamentoMensalidade(unidade.Id);

//            PagamentoMensalidadeWebService.Cadastrar(pagamentoMensalidade, token);
//        }

//        [TestMethod]
//        [TestCategory("BuscarPorId")]
//        public void PagamentoMensalidade_BuscarPorId()
//        {
//            var token = TokenWebService.BuscarToken();
//            var dado = PagamentoMensalidadeWebService.BuscarPorId(id, token);

//            Assert.IsTrue(dado != null);
//        }

//        [TestMethod]
//        [TestCategory("ListarTodos")]
//        public void PagamentoMensalidade_ListarTodos()
//        {
//            var token = TokenWebService.BuscarToken();
//            var dados = PagamentoMensalidadeWebService.Listar(token);

//            Assert.IsTrue(dados.Count() > 0);
//        }

//        [TestMethod]
//        [TestCategory("Editar")]
//        public void PagamentoMensalidade_Editar()
//        {
//            var token = TokenWebService.BuscarToken();
//            var dado = PagamentoMensalidadeWebService.BuscarPorId(id, token);
//            //dado.NumContratoMensalista = "editado";

//            PagamentoMensalidadeWebService.Editar(dado, token);

//            dado = PagamentoMensalidadeWebService.BuscarPorId(id, token);
//            //Assert.IsTrue(dado.NumContratoMensalista == "editado");
//        }

//        [TestMethod]
//        [TestCategory("Deletar")]
//        public void PagamentoMensalidade_Deletar()
//        {
//            var token = TokenWebService.BuscarToken();

//            PagamentoMensalidadeWebService.DeletarPorId(id, token);

//            var dado = PagamentoMensalidadeWebService.BuscarPorId(id, token);

//            Assert.IsTrue(dado == null);
//        }
//    }
//}