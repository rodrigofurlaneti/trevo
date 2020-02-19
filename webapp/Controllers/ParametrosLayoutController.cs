using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Aplicacao;
using Aplicacao.ViewModels;
using Core.Exceptions;
using Entidade;
using Entidade.Uteis;

namespace Portal.Controllers
{
    public class ParametrosLayoutController : GenericController<ParametrosLayout>
    {
        public List<ParametrosLayout> ListaParametrosLayout => Aplicacao?.Buscar()?.ToList() ?? new List<ParametrosLayout>();

        public List<CarteiraViewModel> ListaCarteiras
        {
            get { return (List<CarteiraViewModel>)Session["ListaCarteirasSelecionadas"] ?? new List<CarteiraViewModel>(); }
            set { Session["ListaCarteirasSelecionadas"] = value; }
        }

        //public List<ParametrosCarteiraViewModel> ListaParametrosCarteiras
        //{
        //    get { return (List<ParametrosCarteiraViewModel>)Session["ListaParametrosCarteirasSelecionadas"] ?? new List<ParametrosCarteiraViewModel>(); }
        //    set { Session["ListaParametrosCarteirasSelecionadas"] = value; }
        //}

        public List<LayoutViewModel> ListaLayouts
        {
            get { return (List<LayoutViewModel>)Session["ListaLayoutsSelecionadas"] ?? new List<LayoutViewModel>(); }
            set { Session["ListaLayoutsSelecionadas"] = value; }
        }

        public FormatoCarga FormatoCarga { get; set; }

        private readonly ICarteiraAplicacao _carteiraAplicacao;
        //private readonly IParametrosCarteiraAplicacao _parametrosCarteiraAplicacao;
        private readonly ILayoutAplicacao _layoutAplicacao;
        public ParametrosLayoutController(
                    IParametrosLayoutAplicacao layoutParametrosAplicacao,
                    ICarteiraAplicacao carteiraAplicacao,
                    ILayoutAplicacao layoutAplicacao)
        {
            Aplicacao = layoutParametrosAplicacao;
            _carteiraAplicacao = carteiraAplicacao;
            //_parametrosCarteiraAplicacao = parametrosCarteiraAplicacao;
            _layoutAplicacao = layoutAplicacao;
        }

        [CheckSessionOut]
        public override ActionResult Index()
        {
            ModelState.Clear();
            //ListaParametrosCarteiras = _parametrosCarteiraAplicacao.Buscar().Select(x => new ParametrosCarteiraViewModel(x)).ToList();
            ListaLayouts = _layoutAplicacao.Buscar().Select(x => new LayoutViewModel(x)).ToList();
            ListaCarteiras = _carteiraAplicacao.Buscar().Select(x => new CarteiraViewModel(x)).ToList();
            return View("Index");
        }

        [CheckSessionOut]
        [HttpPost]
        public ActionResult SalvarDados(ParametrosLayoutViewModel layoutParametros)
        {
            try
            {
                Aplicacao.Salvar(layoutParametros.ToEntity());

                ModelState.Clear();

                DadosModal = new DadosModal
                {
                    Titulo = "Sucesso",
                    Mensagem = "Registro salvo com sucesso",
                    TipoModal = TipoModal.Success
                };
            }
            catch (BusinessRuleException br)
            {
                DadosModal = new DadosModal
                {
                    Titulo = "Atenção",
                    Mensagem = br.Message,
                    TipoModal = TipoModal.Warning
                };
            }
            catch (Exception ex)
            {
                DadosModal = new DadosModal
                {
                    Titulo = "Atenção",
                    Mensagem = "Ocorreu um erro ao salvar: " + ex.Message,
                    TipoModal = TipoModal.Danger
                };
            }
            return View("Index");
        }

        public override ActionResult Edit(int id)
        {
            var produto = Aplicacao.BuscarPorId(id);
            return View("Index", new ParametrosLayoutViewModel(produto));
        }
    }
}