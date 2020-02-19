using Aplicacao;
using Aplicacao.Mappers;
using Aplicacao.ViewModels;
using Entidade;
using Entidade.Uteis;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Linq;
using Test.Start;
using WebServices;

namespace Test.Aplicacao
{
    [TestClass]
    public class SeloTests : BaseTests
    {
        private readonly IEmissaoSeloAplicacao _emissaoSeloAplicacao;
        private readonly ISeloSoftparkAplicacao _seloSoftparkAplicacao;

        public SeloTests()
        {
            _emissaoSeloAplicacao = SimpleInjectorInitializer.Container.GetInstance<IEmissaoSeloAplicacao>();
            _seloSoftparkAplicacao = SimpleInjectorInitializer.Container.GetInstance<ISeloSoftparkAplicacao>();
        }

        [TestMethod]
        [TestCategory("Cadastrar")]
        public void Selo_AtualizarSelos()
        {
            var emissaoSelo = _emissaoSeloAplicacao.PrimeiroPor(x => x.StatusSelo == StatusSelo.Ativo);
            var selos = emissaoSelo.Selo.Select(x => new SeloSoftparkViewModel(x)).ToList();

            _seloSoftparkAplicacao.AtualizarSelos(selos);
        }
    }
}