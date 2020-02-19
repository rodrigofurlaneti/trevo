using System.Web.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using Aplicacao;
using Aplicacao.ViewModels;
using Entidade;
using Entidade.Uteis;

namespace Portal.Controllers
{
    [AllowAnonymous]
    public class ProcessamentoController : GenericController<ProcessoInterno>
    {
        public SelectList ListaCarteiras => CarteiraAtivas();

        public List<ParametroCalculoViewModel> ListaParametroCalculo
        {
            get { return (List<ParametroCalculoViewModel>)Session["ListaParametroCalculoProcesso"] ?? new List<ParametroCalculoViewModel>(); }
            set { Session["ListaParametroCalculoProcesso"] = value; }
        }

        private readonly ICarteiraAplicacao _carteiraAplicacao;
        private readonly IProcessoInternoAplicacao _processoInternoAplicacao;
        private readonly IParametroCalculoAplicacao _parametroCalculoAplicacao;

        public ProcessamentoController(IProcessoInternoAplicacao processoInternoAplicacao, ICarteiraAplicacao carteiraAplicacao, IParametroCalculoAplicacao parametroCalculoAplicacao)
        {
            _processoInternoAplicacao = processoInternoAplicacao;
            _carteiraAplicacao = carteiraAplicacao;
            _parametroCalculoAplicacao = parametroCalculoAplicacao;
        }

        private SelectList CarteiraAtivas(int id = 0)
        {
            var lista = new List<CarteiraViewModel>
            {
                new CarteiraViewModel()
                {
                    SiglaDescricao = @"Selecione um...",
                    Id = 0
                }
            };
            var busca = _carteiraAplicacao?.BuscarAtivos()?.Select(x => new CarteiraViewModel(x)).ToList();

            if (busca == null || busca.Count <= 0) return new SelectList(lista, "Id", "SiglaDescricao");

            lista.AddRange(busca);

            return new SelectList(lista, "Id", "SiglaDescricao", id > 0 ? id : 0);
        }

        public ActionResult BuscarParametro(int carteiraId, TipoParametro tipoParametro)
        {
            var lista = _parametroCalculoAplicacao.Buscar().Where(x => x.TipoParametro == tipoParametro && x.Carteiras.FirstOrDefault()?.Id == carteiraId).Select(p => new ParametroCalculoViewModel(p)).ToList();

            Session["ListaParametroCalculoProcesso"] = lista;

            return View("Index");
        }

        /// <summary>
        /// Endereco: Processamento/ProcessarDadosDiario
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        public bool ProcessarDadosDiario()
        {
            try
            {
                return _processoInternoAplicacao.ProcessarDados();
            }
            catch (Exception ex)
            {
                var msg = ex.Message;
                return false;
            }
        }
    }
}