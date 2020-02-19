using Aplicacao;
using Aplicacao.ViewModels;
using Core.Exceptions;
using Entidade;
using Entidade.Uteis;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Portal.Controllers
{
    public class ParametroNegociacaoController : GenericController<ParametroNegociacao>
    {
        public List<LimiteDescontoViewModel> LimitesDesconto => limiteDescontoAplicacao?
                                                                .Buscar()?.Select(x => new LimiteDescontoViewModel(x))?
                                                                .ToList() ?? new List<LimiteDescontoViewModel>();

        public List<UnidadeViewModel> ListaUnidades  => unidadeAplicacao?
                                                                .Buscar()?.Select(x => new UnidadeViewModel(x))?
                                                                .ToList() ?? new List<UnidadeViewModel>();

        public List<PerfilViewModel> ListaPerfis => perfilAplicacao?
                                                                .Buscar()?.Select(x => new PerfilViewModel(x))?
                                                                .ToList() ?? new List<PerfilViewModel>();
        public List<string> ListaTiposValor = Enum.GetValues(typeof(TipoValor))
                                                            .Cast<TipoValor>()
                                                            .Select(v => v.ToString())
                                                            .ToList();

        public List<string> ListaTiposServico = Enum.GetValues(typeof(TipoServico))
                                                           .Cast<TipoServico>()
                                                           .Select(v => v.ToString())
                                                           .ToList();

      
        private readonly IParametroNegociacaoAplicacao parametroNegociacaoAplicacao;
        private readonly ILimiteDescontoAplicacao limiteDescontoAplicacao;
        private readonly IUnidadeAplicacao unidadeAplicacao;
        private readonly IPerfilAplicacao perfilAplicacao;
        public DadosModal DadosModal { get; set; }
        public ParametroNegociacaoController(IParametroNegociacaoAplicacao parametroNegociacaoAplicacao
                                            , ILimiteDescontoAplicacao limiteDescontoAplicacao
                                            , IUnidadeAplicacao unidadeAplicacao
                                            , IPerfilAplicacao perfilAplicacao)
        {
            this.parametroNegociacaoAplicacao = parametroNegociacaoAplicacao;
            this.limiteDescontoAplicacao = limiteDescontoAplicacao;
            this.unidadeAplicacao = unidadeAplicacao;
            this.perfilAplicacao = perfilAplicacao;
        }
        // GET: ParametroNegociacao
        public override ActionResult Index()
        {
            ModelState.Clear();
            var parametroNegociacao = new ParametroNegociacaoViewModel();
            parametroNegociacao.LimitesDesconto = Helpers.ParametroNegociacao.ListaParametroNegociacaoLimiteDesconto(this.ListaTiposServico); 
            return View("Index",parametroNegociacao);
        }

     

        // GET: ParametroNegociacao/Create
        [CheckSessionOut]
        [HttpPost]
        public ActionResult Salva(ParametroNegociacaoViewModel parametroNegociacao)
        {
            parametroNegociacao.LimitesDesconto = (List<LimiteDescontoViewModel>)Session["LimitesDesconto"];
            parametroNegociacaoAplicacao.Salvar(parametroNegociacao.ToEntity());
            ModelState.Clear();
            GerarDadosModal("Sucesso", "Registro salvo com sucesso!", TipoModal.Success);
            var objParametroNegociacao = new ParametroNegociacaoViewModel();
            objParametroNegociacao.LimitesDesconto = Helpers.ParametroNegociacao.ListaParametroNegociacaoLimiteDesconto(this.ListaTiposServico);
            return View("Index", objParametroNegociacao);
        }

        public void SalvaLimiteDesconto(string json)
        {
            var listaLimitesDesconto = new List<LimiteDescontoViewModel>();
            listaLimitesDesconto.AddRange(JsonConvert.DeserializeObject<List<LimiteDescontoViewModel>>(json));
            Session["LimitesDesconto"] = listaLimitesDesconto;
        }
       

        // GET: ParametroNegociacao/Edita/id
        public ActionResult Edita(int id)
        {
          
            var parametroNegociacao = new ParametroNegociacaoViewModel();
            parametroNegociacao = new ParametroNegociacaoViewModel(parametroNegociacaoAplicacao.BuscarPorId(id));
            var limitesDesconto  = Helpers.ParametroNegociacao.ListaParametroNegociacaoLimiteDesconto(this.ListaTiposServico);

            foreach (var limiteDesconto in limitesDesconto)
            {
                if (parametroNegociacao.LimitesDesconto.ToList().Exists(obj => obj.TipoServico == limiteDesconto.TipoServico))
                {
                    continue;
                }
                parametroNegociacao.LimitesDesconto.Add(limiteDesconto);
            }


            return View("Index", parametroNegociacao);
        }


        [CheckSessionOut]
        public override ActionResult Delete(int id)
        {
            try
            {
                GerarDadosModal("Remover registro", "Deseja remover este registro?", TipoModal.Danger, "ConfirmarDelete",
                    "Sim, Desejo remover!", id);
            }
            catch (BusinessRuleException br)
            {
                GerarDadosModal("Atenção", br.Message, TipoModal.Warning);
            }
            catch (Exception ex)
            {
                GerarDadosModal("Atenção", ex.Message, TipoModal.Danger);
            }

            return View("Index");
        }

        [CheckSessionOut]
        public override ActionResult ConfirmarDelete(int id)
        {
            Dictionary<string, object> jsonResult = new Dictionary<string, object>();
            try
            {
                parametroNegociacaoAplicacao.ExcluirPorId(id);

                ModelState.Clear();
                GerarDadosModal("Sucesso", "Removido com sucesso!", TipoModal.Success);
            }
            catch (BusinessRuleException br)
            {
                GerarDadosModal("Atenção", br.Message, TipoModal.Warning);
                jsonResult.Add("Status", "Error");
            }
            catch (Exception ex)
            {
                GerarDadosModal("Atenção", ex.Message, TipoModal.Danger);
                jsonResult.Add("Status", "Error");
            }

            var objParametroNegociacao = new ParametroNegociacaoViewModel();
            objParametroNegociacao.LimitesDesconto = Helpers.ParametroNegociacao.ListaParametroNegociacaoLimiteDesconto(this.ListaTiposServico);
            return View("Index", objParametroNegociacao);
        }

        
        public JsonResult BuscarParametroNegociacao(string unidade)
        {
            var parametrosNegociacao = new List<ParametroNegociacaoViewModel>();
            if (!string.IsNullOrEmpty(unidade))
            {
                parametrosNegociacao = this.parametroNegociacaoAplicacao.BuscarPor(parametroNegociacao => parametroNegociacao.Unidade.Id == int.Parse(unidade)).Select(x=> new ParametroNegociacaoViewModel(x)).ToList();
            }
            else
            {
                parametrosNegociacao = this.parametroNegociacaoAplicacao.Buscar().Select(x => new ParametroNegociacaoViewModel(x)).ToList();
            }

            Dictionary<string, object> jsonResult = new Dictionary<string, object>();
          
            jsonResult.Add("Html", Helpers.RazorHelper.RenderRazorViewToString(ControllerContext, "_GridParametroNegociacao", parametrosNegociacao));
            jsonResult.Add("Status", "Success");
            return Json(jsonResult, JsonRequestBehavior.AllowGet);
        }
    }
}
