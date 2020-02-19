using Aplicacao;
using Aplicacao.ViewModels;
using Core.Exceptions;
using Core.Enums;
using Entidade;
using Entidade.Uteis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using Portal.Helpers;
using Core.Extensions;
using Portal.Models;

namespace Portal.Controllers
{
    public class PedidoLocacaoController : GenericController<PedidoLocacao>
    {
        private readonly IPedidoLocacaoAplicacao _pedidoLocacaoAplicacao;
        private readonly IUnidadeAplicacao _unidadeAplicacao;
        private readonly ITipoLocacaoAplicacao _tipoLocacaoAplicacao;
        private readonly IDescontoAplicacao _descontoAplicacao;
        private readonly IClienteAplicacao _clienteAplicacao;
        private readonly IBloqueioReferenciaAplicacao _bloqueioReferenciaAplicacao;
        private readonly INotificacaoDesbloqueioReferenciaAplicacao _notificacaoDesbloqueioReferenciaAplicacao;

        public List<UnidadeViewModel> ListaUnidade => _unidadeAplicacao.ListarOrdenadoSimplificado()?.Select(x => new UnidadeViewModel(x))?.ToList();
        public List<DescontoViewModel> ListaDesconto => _descontoAplicacao.ListarOrdenado();

        public IEnumerable<ChaveValorViewModel> ListaFormaPagamento => Aplicacao.BuscarValoresDoEnum<FormaPagamento>();
        public IEnumerable<ChaveValorViewModel> ListaPrazoReajuste => Aplicacao.BuscarValoresDoEnum<PrazoReajuste>();
        public IEnumerable<ChaveValorViewModel> ListaTipoReajuste => Aplicacao.BuscarValoresDoEnum<TipoReajuste>();

        public IList<PedidoLocacaoLancamentoAdicionalViewModel> LancamentosAdicionais
        {
            get { return (List<PedidoLocacaoLancamentoAdicionalViewModel>)Session["LancamentosAdicionais"] ?? new List<PedidoLocacaoLancamentoAdicionalViewModel>(); }
            set { Session["LancamentosAdicionais"] = value; }
        }

        public PedidoLocacaoController(
            IPedidoLocacaoAplicacao pedidoLocacaoAplicacao,
            IUnidadeAplicacao unidadeAplicacao,
            ITipoLocacaoAplicacao tipoLocacaoAplicacao,
            IDescontoAplicacao descontoAplicacao,
            IClienteAplicacao clienteAplicacao,
            IBloqueioReferenciaAplicacao bloqueioReferenciaAplicacao,
            INotificacaoDesbloqueioReferenciaAplicacao notificacaoDesbloqueioReferenciaAplicacao)
        {
            Aplicacao = pedidoLocacaoAplicacao;
            _pedidoLocacaoAplicacao = pedidoLocacaoAplicacao;
            _unidadeAplicacao = unidadeAplicacao;
            _tipoLocacaoAplicacao = tipoLocacaoAplicacao;
            _descontoAplicacao = descontoAplicacao;
            _clienteAplicacao = clienteAplicacao;
            _bloqueioReferenciaAplicacao = bloqueioReferenciaAplicacao;
            _notificacaoDesbloqueioReferenciaAplicacao = notificacaoDesbloqueioReferenciaAplicacao;
        }

        [CheckSessionOut]
        public override ActionResult Index()
        {
            return View();
        }

        [CheckSessionOut]
        public override ActionResult Edit(int id)
        {
            var pedidoLocacao = _pedidoLocacaoAplicacao.BuscarPorId(id);
            var pedidoLocacaoViewModel = new PedidoLocacaoViewModel(pedidoLocacao);
            LancamentosAdicionais = pedidoLocacaoViewModel.PedidoLocacaoLancamentosAdicionais;

            return View("Index", pedidoLocacaoViewModel);
        }

        [HttpPost]
        public JsonResult VerificacaoBloqueioReferencia(PedidoLocacaoViewModel pedidoLocacaoViewModel)
        {
            var divModalBloq = string.Empty;
            var data = pedidoLocacaoViewModel.DataPrimeiroPagamento;
            var liberacao = TempData.ContainsKey($"LiberacaoBloqueioReferencia_{Entidades.PedidoLocacao}")
                                ? (DadosValidacaoNotificacaoDesbloqueioReferenciaModal)TempData[$"LiberacaoBloqueioReferencia_{Entidades.PedidoLocacao}"]
                                : new DadosValidacaoNotificacaoDesbloqueioReferenciaModal { StatusDesbloqueioLiberacao = StatusDesbloqueioLiberacao.Aguardando };

            try
            {
                if (liberacao.StatusDesbloqueioLiberacao != StatusDesbloqueioLiberacao.Aprovado)
                {
                    var retorno = _bloqueioReferenciaAplicacao.ValidarLiberacao(liberacao.IdNotificacao, pedidoLocacaoViewModel.Id, Entidades.PedidoLocacao, data, new Usuario { Id = UsuarioLogado.UsuarioId });
                    var modelLiberacao = new DadosValidacaoNotificacaoDesbloqueioReferenciaModal
                    {
                        IdNotificacao = retorno.Key,
                        StatusDesbloqueioLiberacao = retorno.Value,
                        IdRegistro = pedidoLocacaoViewModel.Id,
                        EntidadeRegistro = Entidades.PedidoLocacao,
                        DataReferencia = data,
                        UsuarioLogadoId = UsuarioLogado.UsuarioId,
                        LiberacaoUtilizada = false
                    };
                    TempData[$"LiberacaoBloqueioReferencia_{Entidades.PedidoLocacao}"] = modelLiberacao;
                    TempData.Keep();

                    if (retorno.Value != StatusDesbloqueioLiberacao.Aprovado)
                    {
                        divModalBloq = RazorHelper.RenderRazorViewToString(ControllerContext, "_ModalLiberacaoBloqueioReferencia", modelLiberacao);
                        throw new BlockedReferenceDateException();
                    }
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
                    TipoModal = TipoModal.Warning.ToDescription(),
                    Titulo = "Atenção",
                    Mensagem = "Ocorreu um erro ao salvar: " + ex.Message
                }, JsonRequestBehavior.AllowGet);
            }
            return new JsonResult
            {
                Data = new
                {
                    Bloqueio = false,
                    Modal = divModalBloq,
                    Liberacao = liberacao,
                    Status = liberacao.StatusDesbloqueioLiberacao.ToDescription()
                }
            };
        }

        [HttpPost]
        [CheckSessionOut]
        public ActionResult SalvarDados(PedidoLocacaoViewModel pedidoLocacaoViewModel)
        {
            try
            {
                var liberacao = TempData.ContainsKey($"LiberacaoBloqueioReferencia_{Entidades.PedidoLocacao}")
                                ? (DadosValidacaoNotificacaoDesbloqueioReferenciaModal)TempData[$"LiberacaoBloqueioReferencia_{Entidades.PedidoLocacao}"]
                                : null;

                var usuarioLogadoCurrent = HttpContext.User as CustomPrincipal;
                pedidoLocacaoViewModel.PedidoLocacaoLancamentosAdicionais = LancamentosAdicionais;
                if (pedidoLocacaoViewModel.IdDesconto > 0)
                    pedidoLocacaoViewModel.Desconto = new DescontoViewModel { Id = pedidoLocacaoViewModel.IdDesconto };
                _pedidoLocacaoAplicacao.SalvarPedidoLocacao(pedidoLocacaoViewModel, usuarioLogadoCurrent.UsuarioId);

                if (liberacao != null && liberacao.EntidadeRegistro == Entidades.PedidoLocacao && liberacao.IdRegistro == pedidoLocacaoViewModel.Id && liberacao.IdNotificacao > 0)
                {
                    _notificacaoDesbloqueioReferenciaAplicacao.ConsumirLiberacao(liberacao.IdNotificacao, true);
                    TempData.Remove($"LiberacaoBloqueioReferencia_{Entidades.PedidoLocacao}");
                }

                DadosModal = new DadosModal
                {
                    Titulo = "Sucesso",
                    Mensagem = "Solicitação realizada com sucesso",
                    TipoModal = TipoModal.Success
                };
            }
            catch (BlockedReferenceDateException)
            {
                DadosValidacaoDesbloqueioReferenciaModal = new DadosValidacaoNotificacaoDesbloqueioReferenciaModal
                {
                    Titulo = "Atenção",
                    TipoModal = TipoModal.Info
                };
                return View("Index", pedidoLocacaoViewModel);
            }
            catch (BusinessRuleException br)
            {
                DadosModal = new DadosModal
                {
                    Titulo = "Atenção",
                    Mensagem = br.Message,
                    TipoModal = TipoModal.Warning
                };
                return View("Index", pedidoLocacaoViewModel);
            }
            catch (Exception ex)
            {
                DadosModal = new DadosModal
                {
                    Titulo = "Erro",
                    Mensagem = ex.Message,
                    TipoModal = TipoModal.Danger
                };
                return View("Index", pedidoLocacaoViewModel);
            }

            return View("Index", pedidoLocacaoViewModel);
        }

        public JsonResult BuscarClientesDaUnidade(int unidadeId)
        {
            var chaveValorClientes = _clienteAplicacao.BuscarClientesDaUnidade(unidadeId)?.OrderBy(x=>x.Descricao)?.ToList();

            return Json(chaveValorClientes);
        }

        public ActionResult BuscarPedidosLocacao()
        {
            var pedidosLocacaoViewModel = _pedidoLocacaoAplicacao.ListarPedidoLocacaoFiltro(new PedidoLocacaoViewModel())?.Select(x => new PedidoLocacaoViewModel(x))?.ToList();

            return PartialView("_GridPedidoLocacao", pedidosLocacaoViewModel);
        }

        public ActionResult AtualizarLancamentosAdicionais(List<PedidoLocacaoLancamentoAdicionalViewModel> lancamentosAdicionais)
        {
            LancamentosAdicionais = lancamentosAdicionais;

            return PartialView("_GridPedidoLocacaoLancamentosAdicionais", lancamentosAdicionais ?? new List<PedidoLocacaoLancamentoAdicionalViewModel>());
        }

        public JsonResult BuscarLancamentosAdicionais(List<PedidoLocacaoLancamentoAdicionalViewModel> lancamentosAdicionais)
        {
            return Json(LancamentosAdicionais);
        }

        public ActionResult AtualizarTipoLocacao(int unidadeId)
        {
            var tipoLocacoes = unidadeId > 0 ? _tipoLocacaoAplicacao.BuscarParametrizadosPelaUnidadeId(unidadeId) : new List<TipoLocacaoViewModel>();

            return Json(new
            {
                TemLocacoes = tipoLocacoes.Count > 0,
                Grid = RazorHelper.RenderRazorViewToString(ControllerContext, "_TipoLocacoes", tipoLocacoes)
            }, JsonRequestBehavior.AllowGet);
        }
    }
}