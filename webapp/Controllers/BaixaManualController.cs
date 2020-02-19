using Aplicacao;
using Aplicacao.ViewModels;
using Core.Exceptions;
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
    public class BaixaManualController : GenericController<LancamentoCobranca>
    {
        public List<LancamentoCobranca> ListaLancamentoCobrancas
        {
            get { return (List<LancamentoCobranca>)Session["ListaLancamentoCobrancas"] ?? new List<LancamentoCobranca>(); }
            set { Session["ListaLancamentoCobrancas"] = value; }
        }
        public List<LancamentoCobranca> ListaLancamentosCobrancasSelecionadas
        {
            get { return (List<LancamentoCobranca>)Session["ListaLancamentosCobrancasSelecionadas"] ?? new List<LancamentoCobranca>(); }
            set { Session["ListaLancamentosCobrancasSelecionadas"] = value; }
        }
        public List<ContaFinanceiraViewModel> ListaContaFinanceira
        {
            get { return (List<ContaFinanceiraViewModel>)Session["ListaContaFinanceira"] ?? new List<ContaFinanceiraViewModel>(); }
            set { Session["ListaContaFinanceira"] = value; }
        }

        public IEnumerable<ChaveValorViewModel> ListaTipoServico
        {
            get
            {
                return Enum.GetValues(typeof(TipoServico)).Cast<TipoServico>().Select(e => new ChaveValorViewModel { Id = (int)e, Descricao = e.ToDescription() });
            }
        }
        public IEnumerable<ChaveValorViewModel> ListaFormaPagamento
        {
            get
            {
                return Enum.GetValues(typeof(FormaPagamento)).Cast<FormaPagamento>().Select(e => new ChaveValorViewModel { Id = (int)e, Descricao = e.ToDescription() });
            }
        }

        private readonly ILancamentoCobrancaAplicacao _lancamentoCobrancaAplicacao;
        private readonly IContaFinanceiraAplicacao _contaFinanceiraAplicacao;
        private readonly IClienteAplicacao _clienteAplicacao;
        private readonly IContaContabilAplicacao _contacontabilAplicacao;
        private readonly IBloqueioReferenciaAplicacao _bloqueioReferenciaAplicacao;
        private readonly INotificacaoDesbloqueioReferenciaAplicacao _notificacaoDesbloqueioReferenciaAplicacao;

        public List<ContaContabilViewModel> ListaContaContabil
        {
            get
            {
                return _contacontabilAplicacao?.BuscarDadosSimples()?.Select(x => new ContaContabilViewModel(x))?.ToList();
            }
        }

        public BaixaManualController(ILancamentoCobrancaAplicacao lancamentoCobrancaAplicacao,
                                     IContaFinanceiraAplicacao contaFinanceiraAplicacao,
                                     IClienteAplicacao clienteAplicacao,
                                     IContaContabilAplicacao contacontabil,
                                     IBloqueioReferenciaAplicacao bloqueioReferenciaAplicacao,
                                     INotificacaoDesbloqueioReferenciaAplicacao notificacaoDesbloqueioReferenciaAplicacao)
        {
            Aplicacao = lancamentoCobrancaAplicacao;
            _lancamentoCobrancaAplicacao = lancamentoCobrancaAplicacao;
            _contaFinanceiraAplicacao = contaFinanceiraAplicacao;
            _clienteAplicacao = clienteAplicacao;
            _contacontabilAplicacao = contacontabil;
            _bloqueioReferenciaAplicacao = bloqueioReferenciaAplicacao;
            _notificacaoDesbloqueioReferenciaAplicacao = notificacaoDesbloqueioReferenciaAplicacao;
        }

        [CheckSessionOut]
        public override ActionResult Index()
        {
            ModelState.Clear();

            ListaLancamentosCobrancasSelecionadas = null;
            ListaContaFinanceira = _contaFinanceiraAplicacao.Buscar()?.Select(x => new ContaFinanceiraViewModel(x))?.ToList();
            ListaLancamentoCobrancas = new List<LancamentoCobranca>(); //Aplicacao?.Buscar()?.ToList() ?? new List<LancamentoCobranca>();

            return View("Index");
        }

        [CheckSessionOut]
        public ActionResult Pesquisar(BaixaManualViewModel filtro)
        {
            var lancamentoCobrancasVM = new List<BaixaManualViewModel>();

            string message;
            var tipo = TipoModal.Success;
            decimal totalcontratos = 0;
            decimal totalmultas = 0;
            decimal totaljuros = 0;
            int totallancamentos = 0;
            var lancamentosGrid = string.Empty;

            string totalcontratosF = string.Empty;
            string totalmultasF = string.Empty;
            string totaljurosF = string.Empty;

            try
            {
                message = "Pesquisa feita com sucesso!";

                var lancamentoCobrancaDM = _lancamentoCobrancaAplicacao.ListarLancamentosCobranca(filtro)?.ToList() ?? new List<LancamentoCobranca>();

                lancamentoCobrancasVM = lancamentoCobrancaDM?.Select(x => new BaixaManualViewModel(x))?.ToList();

                ListaLancamentoCobrancas = lancamentoCobrancaDM;
                ListaLancamentosCobrancasSelecionadas = null;

                lancamentosGrid = RetornarLancamentosCobranca(lancamentoCobrancasVM);

                totalcontratos = ListaLancamentoCobrancas.Sum(x => x.ValorContrato);
                totalmultas = ListaLancamentoCobrancas.Sum(x => x.ValorMulta);
                totaljuros = ListaLancamentoCobrancas.Sum(x => x.ValorJuros);
                totallancamentos = ListaLancamentoCobrancas.Count;

                totalcontratosF = totalcontratos.ToString("C");
                totalmultasF = totalmultas.ToString("C");
                totaljurosF = totaljuros.ToString("C");

            }
            catch (Exception ex)
            {
                message = $"Ops! Ocorreu um erro: {ex.Message}";
                tipo = TipoModal.Danger;
            }


            return new JsonResult { Data = new { message, tipo = tipo.ToDescription(), lancamentosGrid, totalcontratosF, totalmultasF, totaljurosF, totallancamentos }, ContentType = "application/json", MaxJsonLength = int.MaxValue };
        }

        [CheckSessionOut]
        public ActionResult PagamentoParcial(List<int> idsLancamentosCobranca)
        {
            var lancamentoCobrancas = new List<BaixaManualViewModel>();
            var lancamentoCobrancasCalculadas = new List<BaixaManualViewModel>();
            
            try
            {
                ListaLancamentosCobrancasSelecionadas = ListaLancamentoCobrancas.Where(x => idsLancamentosCobranca.Any(y => y.Equals(x.Id)))?.ToList() ?? new List<LancamentoCobranca>();
                lancamentoCobrancas = ListaLancamentosCobrancasSelecionadas?.Select(x => new BaixaManualViewModel(x))?.ToList() ?? new List<BaixaManualViewModel>();

                lancamentoCobrancasCalculadas = ListarLancamentoCalculadoJuros(lancamentoCobrancas);
            }
            catch (Exception ex)
            {
                return Json(new Resultado<object>()
                {
                    Sucesso = false,
                    TipoModal = TipoModal.Danger.ToDescription(),
                    Titulo = "Atenção",
                    Mensagem = "Ocorreu um erro ao salvar: " + ex.Message
                }, JsonRequestBehavior.AllowGet);
            }

            return PartialView("_PagamentoParcial", lancamentoCobrancasCalculadas);
        }

        [CheckSessionOut]
        public ActionResult PagamentoTotal(List<int> idsLancamentosCobranca)
        {
            var lancamentoCobrancas = new List<BaixaManualViewModel>();
            var lancamentoCobrancasCalculadas = new List<BaixaManualViewModel>();
            
            try
            {
                ListaLancamentosCobrancasSelecionadas = ListaLancamentoCobrancas.Where(x => idsLancamentosCobranca.Any(y => y.Equals(x.Id)))?.ToList() ?? new List<LancamentoCobranca>();
                lancamentoCobrancas = ListaLancamentosCobrancasSelecionadas?.Select(x => new BaixaManualViewModel(x))?.ToList() ?? new List<BaixaManualViewModel>();
                foreach (var lancamento in lancamentoCobrancas)
                {
                    var receb = _lancamentoCobrancaAplicacao.BuscarPorId(lancamento.Id);

                    if (receb != null && receb.Recebimento != null)
                    {
                        if (receb.Recebimento.Pagamentos != null)
                        {
                            var valor = receb.Recebimento.Pagamentos.Sum(x => x.ValorPago);
                            lancamento.ValorRecebido = valor.ToString("C2");

                        }
                    }

                    if (receb != null && receb.Recebimento != null)
                    {
                        if (receb.Recebimento.Pagamentos != null)
                        {
                            var valor = receb.Recebimento.Pagamentos.Sum(x => x.ValorPago);
                            lancamento.ValorAReceber = valor;

                        }
                    }
                }

                lancamentoCobrancasCalculadas = ListarLancamentoCalculadoJuros(lancamentoCobrancas);
            }
            catch (Exception ex)
            {
                return Json(new Resultado<object>()
                {
                    Sucesso = false,
                    TipoModal = TipoModal.Danger.ToDescription(),
                    Titulo = "Atenção",
                    Mensagem = "Ocorreu um erro ao salvar: " + ex.Message
                }, JsonRequestBehavior.AllowGet);
            }

            return PartialView("_PagamentoTotal", lancamentoCobrancasCalculadas);
        }


        [CheckSessionOut]
        [HttpPost]
        public ActionResult SalvarDados(LancamentoCobrancaViewModel model)
        {
            try
            {
                var entity = Aplicacao.BuscarPorId(model.Id) ?? new LancamentoCobranca();

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
                    Titulo = "Atenção",
                    Mensagem = ex.Message,
                    TipoModal = TipoModal.Danger
                };
                return View("Index", model);
            }

            return View("Index");
        }

        public override ActionResult Edit(int id)
        {
            var cheque = new LancamentoCobrancaViewModel(Aplicacao.BuscarPorId(id));

            return View("Index", cheque);
        }

        private List<BaixaManualViewModel> ListarLancamentoCalculadoJuros(List<BaixaManualViewModel> lancamentosCobrancas)
        {
            var lancamentoCobrancasCalculadas = new List<BaixaManualViewModel>();

            foreach (var lancamento in lancamentosCobrancas)
            {
                var lancamentoCalculado = new BaixaManualViewModel();

                lancamentoCalculado.Cliente = lancamento.Cliente;
                lancamentoCalculado.ContaContabil = lancamento.ContaContabil;
                lancamentoCalculado.ContaFinanceira = lancamento.ContaFinanceira;
                lancamentoCalculado.DataBaixa = lancamento.DataBaixa;
                lancamentoCalculado.DataInsercao = lancamento.DataInsercao;
                lancamentoCalculado.DataPagamento = lancamento.DataPagamento;
                lancamentoCalculado.DataVencimento = lancamento.DataVencimento;
                lancamentoCalculado.DataVencimentoFim = lancamento.DataVencimentoFim;
                lancamentoCalculado.DataVencimentoInicio = lancamento.DataVencimentoInicio;
                lancamentoCalculado.Documento = lancamento.Documento;
                lancamentoCalculado.FormaPagamento = lancamento.FormaPagamento;
                lancamentoCalculado.Id = lancamento.Id;
                lancamentoCalculado.NumeroRecibo = lancamento.NumeroRecibo;
                lancamentoCalculado.Realizabaixa = lancamento.Realizabaixa;
                lancamentoCalculado.TipoServico = lancamento.TipoServico;
                lancamentoCalculado.ValorAReceber = lancamento.ValorAReceber;
                lancamentoCalculado.ValorContrato = lancamento.ValorContrato;
                lancamentoCalculado.ValorMulta = lancamento.ValorMulta;

                if (lancamento.DataVencimento >= DateTime.Today)
                {
                    lancamentoCalculado.ValorJuros = 0;
                    lancamentoCalculado.ValorMulta = 0;
                }
                else if (lancamento.DataBaixa == null)
                {
                    var days = 0;
                    var daysCount = 0;
                    var data = lancamento.DataVencimento;
                    days = lancamento.DataVencimento.Subtract(DateTime.Today).Days;

                    if (days < 0)
                        days = days * -1;

                    for (int i = 1; i <= days; i++)
                    {

                        data = DateTime.Today.AddDays(1);

                        if (data.DayOfWeek != DayOfWeek.Sunday &&
                            data.DayOfWeek != DayOfWeek.Saturday)
                            daysCount++;
                    }
                    lancamentoCalculado.ValorJuros = daysCount * lancamento.ValorJuros;

                }
                else
                {
                    if (lancamento.DataBaixa <= lancamento.DataVencimento)
                    {
                        lancamentoCalculado.ValorJuros = 0;
                        lancamentoCalculado.ValorMulta = 0;
                    }
                    else
                    {
                        lancamentoCalculado.ValorJuros = (lancamento.ValorJuros) * ((lancamento.DataBaixa.Value.Subtract(lancamento.DataVencimento)).Days - 1);
                    }

                }


                lancamentoCalculado.ValorPago = lancamento.ValorPago;
                lancamentoCalculado.ValorRecebido = lancamento.ValorRecebido;
                lancamentoCalculado.ValorTotal = lancamento.ValorTotal;


                lancamentoCobrancasCalculadas.Add(lancamentoCalculado);

            }



            return lancamentoCobrancasCalculadas;

        }

        [CheckSessionOut]
        public JsonResult BuscarLancamentoCobrancas()
        {
            var lancamentoCobrancas = new List<BaixaManualViewModel>();


            string message;
            var tipo = TipoModal.Success;
            decimal totalcontratos = 0;
            decimal totalmultas = 0;
            decimal totaljuros = 0;
            int totallancamentos = 0;
            var lancamentosGrid = string.Empty;

            string totalcontratosF = string.Empty;
            string totalmultasF = string.Empty;
            string totaljurosF = string.Empty;

            try
            {
                message = "Pesquisa feita com sucesso!";

                lancamentoCobrancas = _lancamentoCobrancaAplicacao.ListarLancamentosCobranca(new BaixaManualViewModel()).Select(x => new BaixaManualViewModel(x)).ToList();

                lancamentosGrid = RetornarLancamentosCobranca(lancamentoCobrancas);

                totalcontratos = lancamentoCobrancas.Sum(x => x.ValorContrato);
                totalmultas = lancamentoCobrancas.Sum(x => x.ValorMulta);
                totaljuros = lancamentoCobrancas.Sum(x => x.ValorJuros);

                totallancamentos = ListaLancamentoCobrancas.Count;

                totalcontratosF = totalcontratos.ToString("C");
                totalmultasF = totalmultas.ToString("C");
                totaljurosF = totaljuros.ToString("C");

            }
            catch (Exception ex)
            {
                message = $"Ops! Ocorreu um erro: {ex.Message}";
                tipo = TipoModal.Danger;
            }


            return new JsonResult { Data = new { message, tipo = tipo.ToDescription(), lancamentosGrid, totalcontratosF, totalmultasF, totaljurosF, totallancamentos }, ContentType = "application/json", MaxJsonLength = int.MaxValue };

        }

        private string RetornarLancamentosCobranca(List<BaixaManualViewModel> lancamentosCobrancas)
        {

            return RazorHelper.RenderRazorViewToString(ControllerContext, "_GridBaixaManual", lancamentosCobrancas);
        }

        public ActionResult EfetuarPagamentoParcial(List<BaixaManualViewModel> dados)
        {
            var divModalBloq = string.Empty;
            var model = new BaixaManualViewModel();
            var liberacao = TempData.ContainsKey($"LiberacaoBloqueioReferencia_{Entidades.BaixaManual}")
                                ? (DadosValidacaoNotificacaoDesbloqueioReferenciaModal)TempData[$"LiberacaoBloqueioReferencia_{Entidades.BaixaManual}"]
                                : new DadosValidacaoNotificacaoDesbloqueioReferenciaModal { StatusDesbloqueioLiberacao = StatusDesbloqueioLiberacao.Aguardando };

            try
            {
                if (liberacao.StatusDesbloqueioLiberacao != StatusDesbloqueioLiberacao.Aprovado)
                {
                    var listaDatasCompetencias = dados?.Select(x => x.DataCompetencia.Value)?.Distinct() ?? new List<DateTime>();
                    foreach (var data in listaDatasCompetencias)
                    {
                        model = dados.FirstOrDefault(x => x.DataCompetencia.Value == data);
                        var retorno = _bloqueioReferenciaAplicacao.ValidarLiberacao(liberacao.IdNotificacao,
                                                                                    model.Id,
                                                                                    Entidades.BaixaManual,
                                                                                    data,
                                                                                    new Usuario { Id = UsuarioLogado.UsuarioId });
                        if (retorno.Value == StatusDesbloqueioLiberacao.Aprovado)
                            continue;
                        else
                        {
                            var modelLiberacao = new DadosValidacaoNotificacaoDesbloqueioReferenciaModal
                            {
                                IdNotificacao = retorno.Key,
                                StatusDesbloqueioLiberacao = retorno.Value,
                                IdRegistro = model.Id,
                                EntidadeRegistro = Entidades.BaixaManual,
                                DataReferencia = data,
                                UsuarioLogadoId = UsuarioLogado.UsuarioId,
                                LiberacaoUtilizada = false
                            };
                            TempData[$"LiberacaoBloqueioReferencia_{Entidades.BaixaManual}"] = modelLiberacao;
                            TempData.Keep();

                            divModalBloq = RazorHelper.RenderRazorViewToString(ControllerContext, "_ModalLiberacaoBloqueioReferencia", modelLiberacao);
                            throw new BlockedReferenceDateException();
                        }
                    }
                }

                _lancamentoCobrancaAplicacao.EfetuarPagamentoParcial(dados, new Usuario { Id = UsuarioLogado.UsuarioId });

                if (liberacao != null && liberacao.EntidadeRegistro == Entidades.BaixaManual && liberacao.IdRegistro == model.Id && liberacao.IdNotificacao > 0)
                {
                    _notificacaoDesbloqueioReferenciaAplicacao.ConsumirLiberacao(liberacao.IdNotificacao, true);
                    TempData.Remove($"LiberacaoBloqueioReferencia_{Entidades.BaixaManual}");
                }
            }
            catch (BlockedReferenceDateException)
            {
                DadosValidacaoDesbloqueioReferenciaModal = new DadosValidacaoNotificacaoDesbloqueioReferenciaModal
                {
                    Titulo = "Atenção",
                    TipoModal = TipoModal.Info
                };
                return new JsonResult
                {
                    Data = new
                    {
                        Bloqueio = true,
                        Modal = divModalBloq,
                        Liberacao = liberacao,
                        Status = liberacao.StatusDesbloqueioLiberacao.ToDescription()
                    }
                };
            }
            catch (Exception ex)
            {
                return Json(new Resultado<object>()
                {
                    Sucesso = false,
                    TipoModal = TipoModal.Danger.ToDescription(),
                    Titulo = "Atenção",
                    Mensagem = "Ocorreu um erro ao salvar: " + ex.Message
                }, JsonRequestBehavior.AllowGet);
            }

            return Json(new Resultado<object>()
            {
                Sucesso = false,
                TipoModal = TipoModal.Success.ToDescription(),
                Titulo = "Pagamento Parcial",
                Mensagem = "Efetuado pagamento parcial dos lancamentos."
            }, JsonRequestBehavior.AllowGet);
        }
        public ActionResult EfetuarPagamentoTotal(List<BaixaManualViewModel> dados)
        {
            var divModalBloq = string.Empty;
            var model = new BaixaManualViewModel();
            var liberacao = TempData.ContainsKey($"LiberacaoBloqueioReferencia_{Entidades.BaixaManual}")
                                ? (DadosValidacaoNotificacaoDesbloqueioReferenciaModal)TempData[$"LiberacaoBloqueioReferencia_{Entidades.BaixaManual}"]
                                : new DadosValidacaoNotificacaoDesbloqueioReferenciaModal { StatusDesbloqueioLiberacao = StatusDesbloqueioLiberacao.Aguardando };

            try
            {
                if (liberacao.StatusDesbloqueioLiberacao != StatusDesbloqueioLiberacao.Aprovado)
                {
                    var listaDatasCompetencias = dados?.Select(x => x.DataCompetencia.Value)?.Distinct() ?? new List<DateTime>();
                    foreach (var data in listaDatasCompetencias)
                    {
                        model = dados.FirstOrDefault(x => x.DataCompetencia == data);
                        var retorno = _bloqueioReferenciaAplicacao.ValidarLiberacao(liberacao.IdNotificacao,
                                                                                    model.Id,
                                                                                    Entidades.BaixaManual,
                                                                                    data,
                                                                                    new Usuario { Id = UsuarioLogado.UsuarioId });
                        if (retorno.Value == StatusDesbloqueioLiberacao.Aprovado)
                            continue;
                        else
                        {
                            var modelLiberacao = new DadosValidacaoNotificacaoDesbloqueioReferenciaModal
                            {
                                IdNotificacao = retorno.Key,
                                StatusDesbloqueioLiberacao = retorno.Value,
                                IdRegistro = model.Id,
                                EntidadeRegistro = Entidades.BaixaManual,
                                DataReferencia = data,
                                UsuarioLogadoId = UsuarioLogado.UsuarioId,
                                LiberacaoUtilizada = false
                            };
                            TempData[$"LiberacaoBloqueioReferencia_{Entidades.BaixaManual}"] = modelLiberacao;
                            TempData.Keep();

                            divModalBloq = RazorHelper.RenderRazorViewToString(ControllerContext, "_ModalLiberacaoBloqueioReferencia", modelLiberacao);
                            throw new BlockedReferenceDateException();
                        }
                    }
                }

                _lancamentoCobrancaAplicacao.EfetuarPagamentoTotal(dados, new Usuario { Id = UsuarioLogado.UsuarioId });

                if (liberacao != null && liberacao.EntidadeRegistro == Entidades.BaixaManual && liberacao.IdRegistro == model.Id && liberacao.IdNotificacao > 0)
                {
                    _notificacaoDesbloqueioReferenciaAplicacao.ConsumirLiberacao(liberacao.IdNotificacao, true);
                    TempData.Remove($"LiberacaoBloqueioReferencia_{Entidades.BaixaManual}");
                }
            }
            catch (BlockedReferenceDateException)
            {
                DadosValidacaoDesbloqueioReferenciaModal = new DadosValidacaoNotificacaoDesbloqueioReferenciaModal
                {
                    Titulo = "Atenção",
                    TipoModal = TipoModal.Info
                };
                return new JsonResult
                {
                    Data = new
                    {
                        Bloqueio = true,
                        Modal = divModalBloq,
                        Liberacao = liberacao,
                        Status = liberacao.StatusDesbloqueioLiberacao.ToDescription()
                    }
                };
            }
            catch (Exception ex)
            {
                return Json(new Resultado<object>()
                {
                    Sucesso = false,
                    TipoModal = TipoModal.Danger.ToDescription(),
                    Titulo = "Atenção",
                    Mensagem = "Ocorreu um erro ao salvar: " + ex.Message
                }, JsonRequestBehavior.AllowGet);
            }

            return Json(new Resultado<object>()
            {
                Sucesso = false,
                TipoModal = TipoModal.Success.ToDescription(),
                Titulo = "Pagamento Total",
                Mensagem = "Efetuado pagamento e baixa dos lancamentos."
            }, JsonRequestBehavior.AllowGet);
        }
    }
}
