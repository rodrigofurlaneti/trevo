using Aplicacao;
using Aplicacao.ViewModels;
using Core.Extensions;
using Entidade;
using Entidade.Uteis;
using Portal.Helpers;
using Portal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Portal.Controllers
{
    public class GeracaoCNABLancamentoCobrancaController : GenericController<LancamentoCobranca>
    {
        public List<LancamentoCobranca> ListaLancamentoCobrancas
        {
            get { return (List<LancamentoCobranca>)Session["ListaLancamentoCobrancasGeracaoCNAB"] ?? new List<LancamentoCobranca>(); }
            set { Session["ListaLancamentoCobrancasGeracaoCNAB"] = value; }
        }
        public GeracaoCNABLancamentoCobrancaViewModel GeracaoDadosBoleto
        {
            get { return (GeracaoCNABLancamentoCobrancaViewModel)Session["GeracaoDadosBoleto"] ?? new GeracaoCNABLancamentoCobrancaViewModel(); }
            set { Session["GeracaoDadosBoleto"] = value; }
        }

        #region Listas Dropdown
        public IEnumerable<ChaveValorViewModel> ListaTipoServico => Enum.GetValues(typeof(TipoServico)).Cast<TipoServico>().Select(e => new ChaveValorViewModel { Id = (int)e, Descricao = e.ToDescription() });
        public List<ContaFinanceiraViewModel> ListaContaFinanceira => _contaFinanceira?.Buscar()?.Select(x => new ContaFinanceiraViewModel(x))?.ToList() ?? new List<ContaFinanceiraViewModel>();
        public List<UnidadeViewModel> ListaUnidade => _unidadeAplicacao?.ListarOrdenadoSimplificado()?.Select(x=> new UnidadeViewModel { Id = x.Id, Codigo = x.Codigo, Nome = x.Nome })?.ToList() ?? new List<UnidadeViewModel>();
        public IEnumerable<ChaveValorViewModel> ListaStatusLancamento => Enum.GetValues(typeof(StatusLancamentoCobranca)).Cast<StatusLancamentoCobranca>()
                .Select(e => new ChaveValorViewModel { Id = (int)e, Descricao = e.ToDescription() })
                .Where(x => x.Id == (int)StatusLancamentoCobranca.EmAberto || x.Id == (int)StatusLancamentoCobranca.ErroCNAB || x.Id == (int)StatusLancamentoCobranca.Novo || x.Id == (int)StatusLancamentoCobranca.ACancelar);
        public IEnumerable<ChaveValorViewModel> ListaTipoFiltro => Enum.GetValues(typeof(TipoFiltroGeracaoCNAB)).Cast<TipoFiltroGeracaoCNAB>().Select(e => new ChaveValorViewModel { Id = (int)e, Descricao = e.ToDescription() });
        public List<FuncionarioViewModel> ListaSupervisor => _funcionarioAplicacao?.BuscarComDadosSimples()?
                                                                                    .Select(x => new FuncionarioViewModel(x))?
                                                                                    .ToList() ?? new List<FuncionarioViewModel>();
        #endregion

        #region Variaveis de Camadas
        private readonly ILancamentoCobrancaAplicacao _lancamentoCobrancaAplicacao;
        private readonly IContaFinanceiraAplicacao _contaFinanceira;
        private readonly IUnidadeAplicacao _unidadeAplicacao;
        private readonly IFuncionarioAplicacao _funcionarioAplicacao;
        private readonly IClienteAplicacao _clienteAplicacao;
        #endregion

        public GeracaoCNABLancamentoCobrancaController(
            ILancamentoCobrancaAplicacao lancamentoCobrancaAplicacao,
            IContaFinanceiraAplicacao contaFinanceira,
            IFuncionarioAplicacao funcionarioAplicacao,
        IUnidadeAplicacao unidadeAplicacao,
        IClienteAplicacao clienteAplicacao)
        {
            Aplicacao = lancamentoCobrancaAplicacao;
            _lancamentoCobrancaAplicacao = lancamentoCobrancaAplicacao;
            _contaFinanceira = contaFinanceira;
            _unidadeAplicacao = unidadeAplicacao;
            _funcionarioAplicacao = funcionarioAplicacao;
            _clienteAplicacao = clienteAplicacao;
        }

        [CheckSessionOut]
        public override ActionResult Index()
        {
            ModelState.Clear();

            GeracaoDadosBoleto = null;
            ListaLancamentoCobrancas = null;

            return View("Index");
        }

        [CheckSessionOut]
        public ActionResult BuscarLancamentoCobrancas()
        {
            var lancamentoCobrancas = new List<GeracaoCNABLancamentoCobrancaViewModel>();
            var grid = string.Empty;

            try
            {
                GeracaoDadosBoleto = null;
                ListaLancamentoCobrancas = Aplicacao.Buscar()?.ToList() ?? new List<LancamentoCobranca>();
                lancamentoCobrancas = ListaLancamentoCobrancas?.Select(x => new GeracaoCNABLancamentoCobrancaViewModel(x))?.ToList() ?? new List<GeracaoCNABLancamentoCobrancaViewModel>();
                grid = RazorHelper.RenderRazorViewToString(ControllerContext, "_GridLancamentos", lancamentoCobrancas);
            }
            catch (Exception ex)
            {
                return Json(new Resultado<object>()
                {
                    Sucesso = false,
                    TipoModal = TipoModal.Warning.ToDescription(),
                    Titulo = "Atenção",
                    Mensagem = "Ocorreu um erro ao buscar lancamentos: " + ex.Message
                }, JsonRequestBehavior.AllowGet);
            }

            return new JsonResult
            {
                Data = new
                {
                    Grid = grid,
                },
                ContentType = "application/json",
                MaxJsonLength = int.MaxValue
            };
        }

        [CheckSessionOut]
        public ActionResult RetirarLancamento(int id)
        {
            var lancamentoCobrancas = new List<GeracaoCNABLancamentoCobrancaViewModel>();
            var grid = string.Empty;

            try
            {
                ListaLancamentoCobrancas.Remove(ListaLancamentoCobrancas.FirstOrDefault(x => x.Id == id));
                lancamentoCobrancas = ListaLancamentoCobrancas?.Select(x => new GeracaoCNABLancamentoCobrancaViewModel(x))?.ToList() ?? new List<GeracaoCNABLancamentoCobrancaViewModel>();
                grid = RazorHelper.RenderRazorViewToString(ControllerContext, "_GridLancamentos", lancamentoCobrancas);
            }
            catch (Exception ex)
            {
                return Json(new Resultado<object>()
                {
                    Sucesso = false,
                    TipoModal = TipoModal.Warning.ToDescription(),
                    Titulo = "Atenção",
                    Mensagem = "Ocorreu um erro ao retirar lançamento: " + ex.Message
                }, JsonRequestBehavior.AllowGet);
            }

            return new JsonResult
            {
                Data = new
                {
                    Grid = grid,
                },
                ContentType = "application/json",
                MaxJsonLength = int.MaxValue
            };
        }

        [HttpPost]
        [CheckSessionOut]
        public ActionResult RetirarLancamentosSelecionados(List<string> itens)
        {
            var lancamentoCobrancas = new List<GeracaoCNABLancamentoCobrancaViewModel>();
            var grid = string.Empty;

            try
            {
                foreach (var id in itens)
                {
                    ListaLancamentoCobrancas.Remove(ListaLancamentoCobrancas.FirstOrDefault(x => x.Id == Convert.ToInt32(id)));
                }

                lancamentoCobrancas = ListaLancamentoCobrancas?.Select(x => new GeracaoCNABLancamentoCobrancaViewModel(x))?.ToList() ?? new List<GeracaoCNABLancamentoCobrancaViewModel>();
                grid = RazorHelper.RenderRazorViewToString(ControllerContext, "_GridLancamentos", lancamentoCobrancas);
            }
            catch (Exception ex)
            {
                return Json(new Resultado<object>()
                {
                    Sucesso = false,
                    TipoModal = TipoModal.Warning.ToDescription(),
                    Titulo = "Atenção",
                    Mensagem = "Ocorreu um erro ao retirar os lançamentos selecionados: " + ex.Message
                }, JsonRequestBehavior.AllowGet);
            }

            return new JsonResult
            {
                Data = new
                {
                    Grid = grid,
                },
                ContentType = "application/json",
                MaxJsonLength = int.MaxValue
            };
        }

        public ActionResult Pesquisar(GeracaoCNABLancamentoCobrancaViewModel filtro)
        {
            var lancamentoCobrancas = new List<GeracaoCNABLancamentoCobrancaViewModel>();
            var grid = string.Empty;

            try
            {
                GeracaoDadosBoleto = null;
                ListaLancamentoCobrancas = _lancamentoCobrancaAplicacao.ListarLancamentosCobranca(filtro)?.ToList() ?? new List<LancamentoCobranca>();
                lancamentoCobrancas = ListaLancamentoCobrancas?.Select(x => new GeracaoCNABLancamentoCobrancaViewModel(x))?.ToList() ?? new List<GeracaoCNABLancamentoCobrancaViewModel>();

                grid = RazorHelper.RenderRazorViewToString(ControllerContext, "_GridLancamentos", lancamentoCobrancas);

            }
            catch (Exception ex)
            {
                return Json(new Resultado<object>()
                {
                    Sucesso = false,
                    TipoModal = TipoModal.Warning.ToDescription(),
                    Titulo = "Atenção",
                    Mensagem = "Ocorreu um erro ao pesquisar: " + ex.Message
                }, JsonRequestBehavior.AllowGet);
            }

            return new JsonResult
            {
                Data = new
                {
                    Grid = grid,
                },
                ContentType = "application/json",
                MaxJsonLength = int.MaxValue
            };
        }

        [CheckSessionOut]
        public ActionResult GerarPesquisados(TipoServico? tipoServico, DateTime? dtVencimento, TipoValor tipoValorJuros, decimal? juros, TipoValor tipoValorMulta, decimal? multa)
        {
            var geracao = new GeracaoCNABLancamentoCobrancaViewModel();

            try
            {
                if (ListaLancamentoCobrancas == null || !ListaLancamentoCobrancas.Any())
                {
                    return Json(new Resultado<object>()
                    {
                        Sucesso = false,
                        TipoModal = TipoModal.Warning.ToDescription(),
                        Titulo = "Atenção",
                        Mensagem = "Não possui dados para gerar o(s) boleto(s)"
                    }, JsonRequestBehavior.AllowGet);
                }
                //else if (ListaLancamentoCobrancas.Exists(x => x.PossueCnab == true))
                //{
                //    var lancamentosComCnab = ListaLancamentoCobrancas.Where(x => x.PossueCnab == true).ToList();
                //    var lancamentosComCnabVM = AutoMapper.Mapper.Map<List<LancamentoCobranca>, List<LancamentoCobrancaViewModel>>(lancamentosComCnab);

                //    return PartialView("_ModalLancamentoCNAB", lancamentosComCnabVM);
                //}

                ListaLancamentoCobrancas = ListaLancamentoCobrancas
                    .OrderBy(x => x.Unidade.Nome)
                    .ThenBy(x => (!string.IsNullOrEmpty(x.Cliente.Pessoa.Nome) ? x.Cliente.Pessoa.Nome : x.Cliente.NomeFantasia) + " " + x.Unidade?.Nome).ToList();
                
                geracao = _lancamentoCobrancaAplicacao.GerarBoletosBancariosHtml(ListaLancamentoCobrancas, dtVencimento, tipoValorJuros, juros, tipoValorMulta, multa, ListaLancamentoCobrancas?.FirstOrDefault(x => x.StatusLancamentoCobranca == StatusLancamentoCobranca.ACancelar) != null ?  TipoOcorrenciaCNAB.BAIXA : TipoOcorrenciaCNAB.ENTRADA);
                GeracaoDadosBoleto = geracao;
                
                if (tipoServico.HasValue && tipoServico.Value == TipoServico.Mensalista)
                {
                    geracao.LancamentosAgrupados = ListaLancamentoCobrancas.GroupBy(x => x.Unidade);

                    var listaComprovante = new List<KeyValuePair<int, string>>();
                    var paginas = 1;
                    foreach (var item in geracao.LancamentosAgrupados.Select(x => x.Key.Codigo).Distinct().ToList())
                    {
                        var comprovante = RazorHelper.RenderRazorViewToString(ControllerContext, "_ComprovanteRecebimento", geracao.LancamentosAgrupados.Where(x => x.Key.Codigo == item).ToList());
                        listaComprovante.Add(new KeyValuePair<int, string>(paginas, comprovante));

                        paginas += ListaLancamentoCobrancas.Count(x => x.Unidade.Codigo == item);
                    }

                    geracao.DadosPDF = Core.Tools.GerarPdfCNAB(listaComprovante, geracao.DadosPDF);
                }

            }
            catch (Exception ex)
            {
                return Json(new Resultado<object>()
                {
                    Sucesso = false,
                    TipoModal = TipoModal.Danger.ToDescription(),
                    Titulo = "Atenção",
                    Mensagem = "Ocorreu um erro ao gerar boletos: " + ex.Message
                }, JsonRequestBehavior.AllowGet);
            }

            return PartialView("_BoletosGerados", geracao);
        }

        [CheckSessionOut]
        public void GerarPDF()
        {
            var geracao = GeracaoDadosBoleto;
            Download.DownloadArquivo(geracao.DadosPDF, "boletos", ContentType.pdf);
        }

        [CheckSessionOut]
        public JsonResult GerarPesquisadosModal(DateTime? dtVencimento, TipoValor tipoValorJuros, decimal? juros, TipoValor tipoValorMulta, decimal? multa)
        {
            var geracao = new GeracaoCNABLancamentoCobrancaViewModel();

            try
            {
                geracao = _lancamentoCobrancaAplicacao.GerarBoletosBancariosHtml(ListaLancamentoCobrancas, dtVencimento, tipoValorJuros, juros, tipoValorMulta, multa, TipoOcorrenciaCNAB.ENTRADA);
                GeracaoDadosBoleto = geracao;
            }
            catch (Exception ex)
            {
                return Json(new Resultado<object>()
                {
                    Sucesso = false,
                    TipoModal = TipoModal.Warning.ToDescription(),
                    Titulo = "Atenção",
                    Mensagem = "Ocorreu um erro ao gerar boletos: " + ex.Message
                }, JsonRequestBehavior.AllowGet);
            }

            var boletos = RazorHelper.RenderRazorViewToString(ControllerContext, "_BoletosGerados", geracao.BoletosHtml);
            return new JsonResult
            {
                Data = new
                {
                    Boletos = boletos,
                },
                ContentType = "application/json",
                MaxJsonLength = int.MaxValue
            };
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

        [HttpPost]
        [CheckSessionOut]
        public JsonResult BuscarCliente(string descricao)
        {
            var lista = _clienteAplicacao.BuscarPor(c => c.Pessoa.Nome.Contains(descricao) || c.NomeFantasia.Contains(descricao));

            return Json(lista.Select(c => new
            {
                c.Id,
                Descricao = c.TipoPessoa == TipoPessoa.Fisica ? c.Pessoa.Nome : c.NomeFantasia
            }));
        }
    }
}