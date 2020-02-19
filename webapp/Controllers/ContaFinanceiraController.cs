using Aplicacao;
using Aplicacao.ViewModels;
using Core.Exceptions;
using Core.Extensions;
using Entidade;
using Entidade.Uteis;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Portal.Controllers
{
    public class ContaFinanceiraController : GenericController<ContaFinanceira>
    {
        public List<Empresa> ListaEmpresa => _empresaAplicacao.Buscar().ToList();
        public List<ContaFinanceira> ListaContaFinanceiras;
        public List<BancoViewModel> ListaBanco
        {
            get { return (List<BancoViewModel>)Session["ListaBanco"] ?? new List<BancoViewModel>(); }
            set { Session["ListaBanco"] = value; }
        }

        private readonly IBancoAplicacao _bancoAplicacao;
        private readonly IEmpresaAplicacao _empresaAplicacao;

        public ContaFinanceiraController(IContaFinanceiraAplicacao contaFinanceiraAplicacao, IBancoAplicacao bancoAplicacao, IEmpresaAplicacao empresaAplicacao)
        {
            Aplicacao = contaFinanceiraAplicacao;
            _bancoAplicacao = bancoAplicacao;
            _empresaAplicacao = empresaAplicacao;
        }

        [CheckSessionOut]
        public override ActionResult Index()
        {
            ModelState.Clear();
            ListaContaFinanceiras = Aplicacao?.Buscar()?.ToList() ?? new List<ContaFinanceira>();
            ListaBanco = _bancoAplicacao.Buscar().Select(x => new BancoViewModel(x)).ToList();
            return View("Index");
        }

        public JsonResult SuggestionPerson(string param, bool exact)
        {
            return Json(_bancoAplicacao.BuscarPor(x => !exact && x.Descricao.Contains(param) || x.Descricao == param).Select(x => new BancoViewModel()));
        }

        public JsonResult Supervisores()
        {
            return Json(ListaBanco);
        }

        [HttpPost]
        public void SupervisorSelecionados(string json)
        {
            ListaBanco = JsonConvert.DeserializeObject<List<BancoViewModel>>(json);
        }


        [CheckSessionOut]
        public override ActionResult ConfirmarDelete(int id)
        {
            try
            {
                Aplicacao.ExcluirPorId(id);

                ModelState.Clear();
                GerarDadosModal("Sucesso", "Removido com sucesso!", TipoModal.Success);
            }
            catch (BusinessRuleException br)
            {
                GerarDadosModal("Atenção", br.Message, TipoModal.Warning);
                return View("Index");
            }
            catch (Exception ex)
            {
                GerarDadosModal("Atenção", new BusinessRuleException(ex.Message).Message, TipoModal.Danger);
                return View("Index");
            }


            return View("Index");
        }

        [CheckSessionOut]
        [HttpPost]
        public ActionResult SalvarDados(ContaFinanceiraViewModel contaFinanceiraVM)
        {
            try
            {
                //Somente pode haver uma conta financeira como conta padrão.
                if (contaFinanceiraVM.ContaPadrao)
                {
                    //Atualizar lista
                    ListaContaFinanceiras = Aplicacao?.Buscar()?.ToList() ?? new List<ContaFinanceira>();
                    var contaFin = ListaContaFinanceiras.FirstOrDefault(x => x.ContaPadrao == true && x.Id != contaFinanceiraVM.Id);
                    if (contaFin != null && contaFin.Id > 0)
                    {
                        DadosModal = new DadosModal
                        {
                            Titulo = "Atenção",
                            Mensagem = "Já existe uma conta financeira salva como CONTA PADRÃO. Operação não permitida!",
                            TipoModal = TipoModal.Warning
                        };
                        return View("Index", contaFinanceiraVM);
                    }
                }
                var contaFinanceiraDM = AutoMapper.Mapper.Map<ContaFinanceiraViewModel, ContaFinanceira>(contaFinanceiraVM);

                Aplicacao.Salvar(contaFinanceiraDM);

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
                return View("Index", contaFinanceiraVM);
            }
            catch (Exception ex)
            {
                DadosModal = new DadosModal
                {
                    Titulo = "Sucesso",
                    Mensagem = ex.Message,
                    TipoModal = TipoModal.Danger
                };
                return View("Index", contaFinanceiraVM);
            }

            return View("Index");
        }

        public override ActionResult Edit(int id)
        {
            var contaFinanceira = new ContaFinanceiraViewModel(Aplicacao.BuscarPorId(id));

            return View("Index", contaFinanceira);
        }

        public ActionResult BuscarContaFinanceiras()
        {
            var contaFinanceiraVM = AutoMapper.Mapper.Map<List<ContaFinanceira>, List<ContaFinanceiraViewModel>> (Aplicacao.Buscar().ToList());

            return PartialView("_GridContaFinanceira", contaFinanceiraVM);
        }
    }
}
