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
using APIRest.API;
using System.Threading.Tasks;
using System.Diagnostics;

namespace Test.API
{
    [TestClass]
    public class MovimentacaoAPITests : BaseTests
    {
        private readonly IMovimentacaoAplicacao _movimentacaoAplicacao;
        private const string tela = "Movimentacoes";

        public MovimentacaoAPITests()
        {
            _movimentacaoAplicacao = SimpleInjectorInitializer.Container.GetInstance<IMovimentacaoAplicacao>();
        }

        [TestMethod]
        [TestCategory("Cadastrar")]
        public void Movimentacao_Cadastrar()
        {
            var movimentacao = _movimentacaoAplicacao.PrimeiroPor(x => true);
            var movimentacaoVM = new MovimentacaoSoftparkViewModel(movimentacao);

            Stopwatch stopWatch = new Stopwatch();
            stopWatch.Start();

            Parallel.For(1, 500, index => {
                var novaMovimentacao = movimentacaoVM.Clone();
                novaMovimentacao.Id = index;
                var dados = MovimentacaoAPIWebService.Salvar(novaMovimentacao, tela);
            });

            stopWatch.Stop();

            TimeSpan ts = stopWatch.Elapsed;

            var teste = "";
        }

        [TestMethod]
        [TestCategory("ListarTodos")]
        public void Movimentacao_Listar()
        {
            var count = 0;

            Parallel.For(0, 1000, index => {
                var dados = MovimentacaoAPIWebService.Listar(tela);
                count = dados.Count;
            });

            Assert.IsTrue(count > 0);
        }
    }
}
