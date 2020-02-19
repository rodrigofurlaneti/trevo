using Aplicacao;
using Aplicacao.ViewModels;
using Core.Exceptions;
using Entidade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.Configuration;
using Entidade.Uteis;
using Core.Extensions;

namespace Portal.Controllers
{
    public class SelecaoDespesaController : GenericController<SelecaoDespesa>
    {
        private readonly ISelecaoDespesaAplicacao selecaoDespesaAplicacao;
        private readonly IDespesaContasAPagarAplicacao despesaContasAPagarAplicacao;
        private readonly IContaPagarAplicacao contaspagarAplicacao;
        private readonly IEmpresaAplicacao _empresaAplicacao;
        private readonly IUnidadeAplicacao unidadeAplicacao;

        public SelecaoDespesaController(ISelecaoDespesaAplicacao selecaoDespesaAplicacao,
                                        IDespesaContasAPagarAplicacao despesaContasAPagarAplicacao,
                                        IContaPagarAplicacao contaspagarAplicacao,
                                        IEmpresaAplicacao empresaAplicacao,
                                        IUnidadeAplicacao unidadeAplicacao)
        {
            this.selecaoDespesaAplicacao = selecaoDespesaAplicacao;
            this.despesaContasAPagarAplicacao = despesaContasAPagarAplicacao;
            this.contaspagarAplicacao = contaspagarAplicacao;
            this._empresaAplicacao = empresaAplicacao;
            this.unidadeAplicacao = unidadeAplicacao;

            ViewBag.ListaMes = new SelectList(
                Enum.GetValues(typeof(Mes)).Cast<Mes>().Select(e => new ChaveValorViewModel { Id = (int)e, Descricao = e.ToDescription() }) ?? new List<ChaveValorViewModel>(),
                "Id",
                "Descricao");

            int qtdeAnos = Convert.ToInt32(ConfigurationManager.AppSettings["qtdeAnos"].ToString());
            ViewBag.ListaAnos = new SelectList(Utils.UtilsServico.ListaAnosAnteriores(DateTime.Now.Year, qtdeAnos));
        }
        public List<EmpresaViewModel> ListaEmpresaUnidade { get; set; }
        public List<ContasAPagarViewModel> ListaContasAPagar { get; set; }
        public List<UnidadeViewModel> ListaUnidade { get; set; }
        public List<ContasAPagarViewModel> ListaDespesas { get; set; }

        // GET: SelecaoDespesa
        public override ActionResult Index()
        {
            ModelState.Clear();

            ListaEmpresaUnidade = AutoMapper.Mapper.Map<List<Empresa>, List<EmpresaViewModel>>(_empresaAplicacao.Buscar().ToList())?.OrderBy(x => x.RazaoSocial)?.ToList() ?? new List<EmpresaViewModel>();
            ViewBag.ListaAnos = new SelectList(Utils.UtilsServico.ListaAnosAnteriores(DateTime.Now.Year, 5));

            ListaContasAPagar = AutoMapper.Mapper.Map<List<ContasAPagar>, List<ContasAPagarViewModel>>(contaspagarAplicacao.Buscar().ToList());
            Session["ContasAPagar"] = ListaContasAPagar;

            ListaUnidade = AutoMapper.Mapper.Map<List<Unidade>, List<UnidadeViewModel>>(unidadeAplicacao.Buscar().ToList());
            ViewBag.ListaUnidade = ListaUnidade;
            Session["Unidades"] = ListaUnidade;
            return View();
        }

        [CheckSessionOut]
        [HttpPost]
        public PartialViewResult BuscarDespesas(int? idEmpresa, int? idUnidade, int ano, int mes)
        {
            ListaDespesas = Filtro(idEmpresa, idUnidade, ano, mes);
            Session["ListaDespesas"] = ListaDespesas;
            return PartialView("_GridDespesas", ListaDespesas);
        }

        public JsonResult BuscarUnidades(int? idEmpresa)
        {
            if (ListaUnidade == null)
                ListaUnidade = (List<UnidadeViewModel>)Session["Unidades"];
            if (idEmpresa != null)
            {
                ListaUnidade = ListaUnidade.Where(x => x.Empresa != null && x.Empresa.Id == idEmpresa).ToList();
                ViewBag.ListaUnidade = ListaUnidade;
                return Json(true, JsonRequestBehavior.AllowGet);
            }
            return Json(false, JsonRequestBehavior.AllowGet);
        }

        private List<ContasAPagarViewModel> Filtro(int? idEmpresa, int? idUnidade, int ano, int mes)
        {
            ListaDespesas = new List<ContasAPagarViewModel>();
            if (ListaContasAPagar == null)
                ListaContasAPagar = (List<ContasAPagarViewModel>)Session["ContasAPagar"];
            if (idEmpresa != null)
            {
                ListaDespesas = (from r in ListaContasAPagar where (r?.Unidade != null && (r?.Unidade?.Empresa?.Id ?? 0) == idEmpresa) select r).ToList();
            }

            if (idUnidade != null)
            {
                ListaDespesas = (from r in ListaDespesas where r.Unidade.Id == idUnidade select r).ToList();
            }

            ListaDespesas = ListaDespesas.Where(x => x.DataPagamento.Month == mes && x.DataPagamento.Year == ano).ToList();
            return ListaDespesas;
        }

        [CheckSessionOut]
        [HttpPost]
        private void AtualizarDespesas(int[] DespesasAIgnorar)
        {
            ListaDespesas = (List<ContasAPagarViewModel>)Session["ListaDespesas"];
            if (DespesasAIgnorar != null)
            {
                for (int i = 0; i < DespesasAIgnorar.Length; i++)
                {
                    if (ListaDespesas != null)
                    {
                        var item = ListaDespesas.SingleOrDefault(x => x.Id == DespesasAIgnorar[i]);
                        item.Ignorado = true;
                        var indexOf = ListaDespesas.IndexOf(ListaDespesas.Find(x => x.Id == item.Id));
                        ListaDespesas[indexOf] = item;
                    }
                }
                Session["ListaDespesas"] = ListaDespesas;
            }
        }

        [CheckSessionOut]
        [HttpPost]
        public ActionResult SalvarDados(SelecaoDespesaViewModel sdVM)
        {
            try
            {
                var sdEntity = AutoMapper.Mapper.Map<SelecaoDespesaViewModel, SelecaoDespesa>(sdVM);
                sdEntity.Despesas = new List<DespesaContasAPagar>();

                if (sdEntity.Unidade.Id == 0)
                    sdEntity.Unidade = unidadeAplicacao.BuscarPor(x => x.Empresa.Id == sdEntity.Empresa.Id).FirstOrDefault();

                ListaDespesas = (List<ContasAPagarViewModel>)Session["ListaDespesas"];
                if (ListaDespesas != null)
                {
                    var listDepesasASalvar = AutoMapper.Mapper.Map<List<ContasAPagarViewModel>, List<ContasAPagar>>(ListaDespesas.FindAll(x => x.Ignorado != true));
                    foreach (var item in listDepesasASalvar)
                    {
                        var teste = contaspagarAplicacao.BuscarPorId(item.Id);
                        DespesaContasAPagar obj = new DespesaContasAPagar()
                        {
                            ContaAPagar = teste
                        };
                        sdEntity.Despesas.Add(obj);
                    }
                    selecaoDespesaAplicacao.Salvar(sdEntity);

                    DadosModal = new DadosModal
                    {
                        Titulo = "Sucesso",
                        Mensagem = "Registro salvo com sucesso",
                        TipoModal = TipoModal.Success
                    };
                }
            }
            catch (BusinessRuleException br)
            {
                DadosModal = new DadosModal
                {
                    Titulo = "Atenção",
                    Mensagem = br.Message,
                    TipoModal = TipoModal.Warning
                };
                return View("Index", sdVM);
            }
            catch (Exception ex)
            {
                DadosModal = new DadosModal
                {
                    Titulo = "Atenção",
                    Mensagem = "Ocorreu um erro ao salvar: " + ex.Message,
                    TipoModal = TipoModal.Danger
                };
                return View("Index", sdVM);
            }
            return RedirectToAction("Index");
        }
    }
}