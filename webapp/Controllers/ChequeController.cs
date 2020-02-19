using Aplicacao;
using Aplicacao.ViewModels;
using Core.Exceptions;
using Entidade;
using Newtonsoft.Json;
using Portal.Servico;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Portal.Controllers
{
    public class ChequeController : GenericController<Cheque>
    {
        public List<Cheque> ListaCheques => Aplicacao?.Buscar()?.ToList() ?? new List<Cheque>();
        public List<ContaFinanceiraViewModel> ListaContaFinanceira
        {
            get { return (List<ContaFinanceiraViewModel>)Session["ListaContaFinanceira"] ?? new List<ContaFinanceiraViewModel>(); }
            set { Session["ListaContaFinanceira"] = value; }
        }

        public List<BancoViewModel> ListaBanco
        {
            get { return (List<BancoViewModel>)Session["Banco"] ?? new List<BancoViewModel>(); }
            set { Session["Banco"] = value; }
        }



        private readonly IContaFinanceiraAplicacao _contaFinanceiraAplicacao;
        private readonly IBancoAplicacao _bancoAplicacao;

        public ChequeController(IChequeAplicacao marcaAplicacao, IContaFinanceiraAplicacao contaFinanceiraAplicacao, IBancoAplicacao bancoAplicacao)
        {
            Aplicacao = marcaAplicacao;
            _contaFinanceiraAplicacao = contaFinanceiraAplicacao;
            _bancoAplicacao = bancoAplicacao;
        }

        [CheckSessionOut]
        public override ActionResult Index()
        {
            ModelState.Clear();
            ListaContaFinanceira = _contaFinanceiraAplicacao.Buscar().Select(x => new ContaFinanceiraViewModel(x)).ToList();
            ListaBanco = EntidadesServico.ListarBanco(_bancoAplicacao);

            return View("Index");
        }

        public JsonResult SuggestionPerson(string param, bool exact)
        {
            return Json(_contaFinanceiraAplicacao.BuscarPor(x => !exact && x.Descricao.Contains(param) || x.Descricao == param).Select(x => new ContaFinanceiraViewModel()));
        }

        public JsonResult Supervisores()
        {
            return Json(ListaContaFinanceira);
        }

        [HttpPost]
        public void SupervisorSelecionados(string json)
        {
            ListaContaFinanceira = JsonConvert.DeserializeObject<List<ContaFinanceiraViewModel>>(json);
        }

        [CheckSessionOut]
        [HttpPost]
        public ActionResult SalvarDados(ChequeViewModel model)
        {
            try
            {
                var entity = Aplicacao.BuscarPorId(model.Id) ?? new Cheque();

                entity = model.ToEntity();
                
                Aplicacao.Salvar(entity);

                ModelState.Clear();
                Session["ImgAvatar"] = null;

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
                return View("Index", model);
            }
            catch (Exception ex)
            {
                DadosModal = new DadosModal
                {
                    Titulo = "Sucesso",
                    Mensagem = ex.Message,
                    TipoModal = TipoModal.Danger
                };
                return View("Index", model);
            }

            return View("Index");
        }

        public override ActionResult Edit(int id)
        {
            var cheque = new ChequeViewModel(Aplicacao.BuscarPorId(id));

            return View("Index", cheque);
        }

        public ActionResult BuscarCheques()
        {
            var cheques = new List<ChequeViewModel>();
            
            cheques = Aplicacao.Buscar().Select(x => new ChequeViewModel(x)).ToList();
            
            return PartialView("_GridCheque", cheques);
        }
    }
}
