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
//    public class DescontoTipoTests : BaseTests
//    {
//        private readonly IUnidadeAplicacao _unidadeAplicacao;

//        public DescontoTipoTests()
//        {
//            _unidadeAplicacao = SimpleInjectorInitializer.Container.GetInstance<IUnidadeAplicacao>();
//        }

//        [TestMethod]
//        [TestCategory("Cadastrar")]
//        public void DescontoTipo_Cadastrar()
//        {
//            var token = TokenWebService.BuscarToken();
//            var unidade = _unidadeAplicacao.PrimeiroPor(x => true);

//            var descontoTipoVM = new DescontoTipoViewModel()
//            {
//                DataInsercao = DateTime.Now,
//                Id = 1,
//                IsHora = true,
//                IsPercentual = true,
//                IsValorFixo = true,
//                Nome = "Teste Trevo",
//                TabelaId = 1,
//                UnidadeId = unidade.Id,
//                Valor = 100,
//                ValorFaturamento = 200
//            };
//            DescontoTipoWebService.Cadastrar(descontoTipoVM, token);
//        }

//        [DataTestMethod]
//        [TestCategory("BuscarPorId")]
//        [DataRow(1)]
//        public void DescontoTipo_BuscarPorId(int id)
//        {
//            var token = TokenWebService.BuscarToken();
//            var dado = DescontoTipoWebService.BuscarPorId(id, token);

//            Assert.IsTrue(dado != null);
//        }

//        [TestMethod]
//        [TestCategory("ListarTodos")]
//        public void DescontoTipo_ListarTodos()
//        {
//            var token = TokenWebService.BuscarToken();
//            var dados = DescontoTipoWebService.Listar(token);

//            Assert.IsTrue(dados.Count() > 0);
//        }

//        [DataTestMethod]
//        [TestCategory("Editar")]
//        [DataRow(1)]
//        public void DescontoTipo_Editar(int id)
//        {
//            var token = TokenWebService.BuscarToken();
//            var dado = DescontoTipoWebService.BuscarPorId(id, token);
//            dado.IsHora = false;
//            dado.IsPercentual = false;
//            dado.IsValorFixo = false;

//            DescontoTipoWebService.Editar(dado, token);

//            dado = DescontoTipoWebService.BuscarPorId(id, token);
//            Assert.IsTrue(!dado.IsHora && !dado.IsPercentual && !dado.IsValorFixo);
//        }

//        [DataTestMethod]
//        [TestCategory("Deletar")]
//        [DataRow(1)]
//        public void DescontoTipo_Deletar(int id)
//        {
//            var token = TokenWebService.BuscarToken();

//            DescontoTipoWebService.DeletarPorId(id, token);

//            var dado = DescontoTipoWebService.BuscarPorId(id, token);

//            Assert.IsTrue(dado == null);
//        }
//    }
//}