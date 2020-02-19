using Aplicacao;
using Aplicacao.ViewModels;
using Core.Extensions;
using Entidade;
using Entidade.Uteis;
using Ionic.Zip;
using Portal.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace Portal.Controllers
{
    public class GeracaoCNABContasPagarController : GenericController<ContasAPagar>
    {
        public List<ContasAPagar> ListaContaPagar
        {
            get { return (List<ContasAPagar>)Session["ListaContaPagarGeracaoCNABContasPagar"] ?? new List<ContasAPagar>(); }
            set { Session["ListaContaPagarGeracaoCNABContasPagar"] = value; }
        }

        public GeracaoCNABContaPagarViewModel GeracaoDadosBoleto
        {
            get { return (GeracaoCNABContaPagarViewModel)Session["GeracaoDadosBoleto"] ?? new GeracaoCNABContaPagarViewModel(); }
            set { Session["GeracaoDadosBoleto"] = value; }
        }

        public List<ContaFinanceira> ListaContaFinanceira => _contaFinanceira?.Buscar().ToList();
        public List<Unidade> ListaUnidade => _unidadeAplicacao?.Buscar().ToList();
        public List<ContaContabil> ListaContaContabil => _contaContabilAplicacao?.Buscar().ToList();
        public List<ChaveValorViewModel> ListaFormaPagamento => _contaPagarAplicacao?.BuscarValoresDoEnum<FormaPagamento>()
                                                                    .Where(x => x.Id == 5 || x.Id == 6 || x.Id == 7 || x.Id == 13 || x.Id == 14).ToList();
        public List<ChaveValorViewModel> ListaTipoFiltro => _contaPagarAplicacao?.BuscarValoresDoEnum<TipoFiltroGeracaoCNAB>().ToList();
        public List<Fornecedor> ListaFornecedor => _fornecedorAplicacao?.Buscar().ToList();

        private readonly IContaPagarAplicacao _contaPagarAplicacao;
        private readonly IContaFinanceiraAplicacao _contaFinanceira;
        private readonly IContaContabilAplicacao _contaContabilAplicacao;
        private readonly IFornecedorAplicacao _fornecedorAplicacao;
        private readonly IUnidadeAplicacao _unidadeAplicacao;

        public GeracaoCNABContasPagarController(
            IContaPagarAplicacao contaPagarAplicacao
            , IContaFinanceiraAplicacao contaFinanceira
            , IContaContabilAplicacao contaContabilAplicacao
            , IFornecedorAplicacao fornecedorAplicacao
            , IUnidadeAplicacao unidadeAplicacao)
        {
            Aplicacao = contaPagarAplicacao;
            _contaPagarAplicacao = contaPagarAplicacao;
            _contaFinanceira = contaFinanceira;
            _contaContabilAplicacao = contaContabilAplicacao;
            _fornecedorAplicacao = fornecedorAplicacao;
            _unidadeAplicacao = unidadeAplicacao;
        }

        [CheckSessionOut]
        public override ActionResult Index()
        {
            ModelState.Clear();

            GeracaoDadosBoleto = null;
            ListaContaPagar = null;

            return View("Index");
        }

        [CheckSessionOut]
        public ActionResult RetirarConta(int id)
        {
            var contasPagar = new List<GeracaoCNABContaPagarViewModel>();

            try
            {
                ListaContaPagar.Remove(ListaContaPagar.FirstOrDefault(x => x.Id == id));
                contasPagar = ListaContaPagar?.Select(x => new GeracaoCNABContaPagarViewModel(x))?.ToList() ?? new List<GeracaoCNABContaPagarViewModel>();
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    Sucesso = false,
                    DadosModal = CriarDadosModalErro($"Ocorreu um erro ao retirar conta: {ex.Message}")
                }, JsonRequestBehavior.AllowGet);
            }

            return PartialView("_GridContas", contasPagar);
        }

        [HttpPost]
        [CheckSessionOut]
        public ActionResult RetirarContasSelecionadas(List<string> itens)
        {
            var contasPagar = new List<GeracaoCNABContaPagarViewModel>();

            try
            {
                foreach (var id in itens)
                {
                    ListaContaPagar.Remove(ListaContaPagar.FirstOrDefault(x => x.Id == Convert.ToInt32(id)));
                }

                contasPagar = ListaContaPagar?.Select(x => new GeracaoCNABContaPagarViewModel(x))?.ToList() ?? new List<GeracaoCNABContaPagarViewModel>();
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    Sucesso = false,
                    DadosModal = CriarDadosModalErro($"Ocorreu um erro ao retirar as contas selecionadas: {ex.Message}")
                }, JsonRequestBehavior.AllowGet);
            }

            return PartialView("_GridContas", contasPagar);
        }

        public ActionResult Pesquisar(GeracaoCNABContaPagarViewModel filtro)
        {
            var contasPagar = new List<GeracaoCNABContaPagarViewModel>();

            try
            {
                ListaContaPagar = _contaPagarAplicacao.BuscarPeloFiltro(filtro)?.ToList() ?? new List<ContasAPagar>();
                contasPagar = ListaContaPagar?.Select(x => new GeracaoCNABContaPagarViewModel(x))?.ToList() ?? new List<GeracaoCNABContaPagarViewModel>();
            }
            catch (Exception ex)
            {
                return Json(new
                {
                    Sucesso = false,
                    DadosModal = CriarDadosModalErro("Ocorreu um erro ao pesquisar: " + ex.Message)
                }, JsonRequestBehavior.AllowGet);
            }

            return PartialView("_GridContas", contasPagar);
        }

        [CheckSessionOut]
        public ActionResult GerarPesquisados(GeracaoCNABContaPagarViewModel filtro)
        {
            try
            {
                if (ListaContaPagar == null || !ListaContaPagar.Any())
                {
                    return Json(new
                    {
                        Sucesso = false,
                        DadosModal = CriarDadosModalErro("Nao possui dados para gerar o cnab")
                    }, JsonRequestBehavior.AllowGet);
                }

                var fornecedorComTedDoc = ListaContaPagar.FirstOrDefault(x => (x.FormaPagamento == FormaPagamento.Ted || x.FormaPagamento == FormaPagamento.Doc) &&
                    (string.IsNullOrEmpty(x.Fornecedor.Agencia) || string.IsNullOrEmpty(x.Fornecedor.Conta)));
                if (fornecedorComTedDoc != null)
                {
                    var fornecedor = _fornecedorAplicacao.BuscarPorId(fornecedorComTedDoc.Fornecedor.Id);
                    return Json(new
                    {
                        Sucesso = false,
                        DadosModal = CriarDadosModalErro($"Complete as informações do Fornecedor {fornecedor.Descricao} para fazer pagamento de TED ou DOC.")
                    }, JsonRequestBehavior.AllowGet);
                }

                GeracaoDadosBoleto = _contaPagarAplicacao.GerarBoletosBancariosHtml(ListaContaPagar, UsuarioLogado.UsuarioId, filtro);

                return Json(new
                {
                    Sucesso = true,
                    DadosModal = CriarDadosModalSucesso("Cnab gerado com sucesso")
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new 
                {
                    Sucesso = false,
                    DadosModal = CriarDadosModalErro("Ocorreu um erro ao gerar o cnab: " + ex.Message)
                }, JsonRequestBehavior.AllowGet);
            }
        }

        [CheckSessionOut]
        public ActionResult GerarArquivoRemessa()
        {
            try
            {
                using (var mem = GeracaoDadosBoleto.ArquivoRemessaMemoryStream)
                {
                    return File(mem.ToArray(), "application/x-msdownload", "arquivoRemessa.txt");
                }
            }
            catch (Exception ex)
            {
                return Json(new Resultado<object>()
                {
                    Sucesso = false,
                    TipoModal = TipoModal.Warning.ToDescription(),
                    Titulo = "Atenção",
                    Mensagem = "Ocorreu um erro ao gerar arquivo de remessa: " + ex.Message
                }, JsonRequestBehavior.AllowGet);
            }
        }
    }
}