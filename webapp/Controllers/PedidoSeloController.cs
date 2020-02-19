using Aplicacao;
using Aplicacao.ViewModels;
using Core.Exceptions;
using Core.Extensions;
using Entidade;
using Entidade.Uteis;
using Portal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Portal.Controllers
{
    public class PedidoSeloController : GenericController<PedidoSelo>
    {
        public IList<ClienteViewModel> ListaCliente
        {
            get { return (List<ClienteViewModel>)TempData["ListaClientePedidoSelo"] ?? new List<ClienteViewModel>(); }
            set { TempData["ListaClientePedidoSelo"] = value; }
        }

        public IList<ConvenioViewModel> ListaConvenio
        {
            get { return (List<ConvenioViewModel>)TempData["ListaConvenioPedidoSelo"] ?? new List<ConvenioViewModel>(); }
            set { TempData["ListaConvenioPedidoSelo"] = value; }
        }

        public IList<UnidadeViewModel> ListaUnidade
        {
            get { return (List<UnidadeViewModel>)TempData["ListaUnidadePedidoSelo"] ?? new List<UnidadeViewModel>(); }
            set { TempData["ListaUnidadePedidoSelo"] = value; }
        }

        public IList<DescontoViewModel> ListaNegociacaoSeloDesconto
        {
            get { return (List<DescontoViewModel>)TempData["ListaNegociacaoSeloDescontoPedidoSelo"] ?? new List<DescontoViewModel>(); }
            set { TempData["ListaNegociacaoSeloDescontoPedidoSelo"] = value; }
        }

        public IList<TipoSeloViewModel> ListaTipoSelo
        {
            get { return (List<TipoSeloViewModel>)TempData["ListaTipoSeloPedidoSelo"] ?? new List<TipoSeloViewModel>(); }
            set { TempData["ListaTipoSeloPedidoSelo"] = value; }
        }

        public IList<PropostaViewModel> ListaProposta
        {
            get { return (List<PropostaViewModel>)TempData["ListaPropostaPedidoSelo"] ?? new List<PropostaViewModel>(); }
            set { TempData["ListaPropostaPedidoSelo"] = value; }
        }

        public IList<ChaveValorViewModel> ListaTipoPagamento
        {
            get { return (List<ChaveValorViewModel>)TempData["ListaTipoPagamentoPedidoSelo"] ?? new List<ChaveValorViewModel>(); }
            set { TempData["ListaTipoPagamentoPedidoSelo"] = value; }
        }

        public IList<ChaveValorViewModel> ListaTipoPedidoSelo
        {
            get { return (List<ChaveValorViewModel>)TempData["ListaTipoPedidoSeloPedidoSelo"] ?? new List<ChaveValorViewModel>(); }
            set { TempData["ListaTipoPedidoSeloPedidoSelo"] = value; }
        }

        public IList<ChaveValorViewModel> ListaStatusPedido
        {
            get { return (List<ChaveValorViewModel>)TempData["ListaStatusPedidoPedidoSelo"] ?? new List<ChaveValorViewModel>(); }
            set { TempData["ListaStatusPedidoPedidoSelo"] = value; }
        }

        public StatusLancamentoCobranca? StatusLancamentoCobrancaPedidoSelo {
            get { return (StatusLancamentoCobranca?)(TempData["StatusLancamentoCobrancaPedidoSelo"] ?? null); }
            set { TempData["StatusLancamentoCobrancaPedidoSelo"] = value; }
        }

        private readonly IPedidoSeloAplicacao _pedidoSeloAplicacao;
        private readonly ILancamentoCobrancaPedidoSeloAplicacao _lancamentoCobrancaPedidoSeloAplicacao;
        private readonly ITipoNotificacaoAplicacao _tipoNotificacaoAplicacao;
        private readonly IUsuarioAplicacao _usuarioAplicacao;
        private readonly IClienteAplicacao _clienteAplicacao;
        private readonly IBloqueioReferenciaAplicacao _bloqueioReferenciaAplicacao;
        private readonly INotificacaoDesbloqueioReferenciaAplicacao _notificacaoDesbloqueioReferenciaAplicacao;

        public PedidoSeloController(
            IPedidoSeloAplicacao pedidoSeloAplicacao,
            ITipoNotificacaoAplicacao tipoNotificacaoAplicacao,
            IUsuarioAplicacao usuarioAplicacao,
            IClienteAplicacao clienteAplicacao,
            IBloqueioReferenciaAplicacao bloqueioReferenciaAplicacao,
            ILancamentoCobrancaPedidoSeloAplicacao lancamentoCobrancaPedidoSeloAplicacao,
            INotificacaoDesbloqueioReferenciaAplicacao notificacaoDesbloqueioReferenciaAplicacao)
        {
            _pedidoSeloAplicacao = pedidoSeloAplicacao;
            _clienteAplicacao = clienteAplicacao;
            _bloqueioReferenciaAplicacao = bloqueioReferenciaAplicacao;
            _notificacaoDesbloqueioReferenciaAplicacao = notificacaoDesbloqueioReferenciaAplicacao;
            _lancamentoCobrancaPedidoSeloAplicacao = lancamentoCobrancaPedidoSeloAplicacao;

            //GTE-1796
            Aplicacao = pedidoSeloAplicacao;

             _tipoNotificacaoAplicacao = tipoNotificacaoAplicacao;
            _usuarioAplicacao = usuarioAplicacao;
        }

        public override ActionResult Index()
        {
            PrepararTela();
            return View(new PedidoSeloViewModel { ValidadePedido = DateTime.Now.Date.AddDays(1) });
        }

        public override ActionResult Edit(int id)
        {
            var pedidoSelo = _pedidoSeloAplicacao.BuscarPorId(id);
            var pedidoSeloVM = new PedidoSeloViewModel(pedidoSelo);
            PrepararTela(pedidoSelo);

            return View("Index", pedidoSeloVM);
        }

        [HttpPost]
        public JsonResult VerificacaoBloqueioReferencia(PedidoSeloViewModel pedidoSelo)
        {
            var divModalBloq = string.Empty;
            var data = pedidoSelo.DataVencimento;
            var liberacao = TempData.ContainsKey($"LiberacaoBloqueioReferencia_{Entidades.PedidoSelo}")
                                ? (DadosValidacaoNotificacaoDesbloqueioReferenciaModal)TempData[$"LiberacaoBloqueioReferencia_{Entidades.PedidoSelo}"]
                                : new DadosValidacaoNotificacaoDesbloqueioReferenciaModal { StatusDesbloqueioLiberacao = StatusDesbloqueioLiberacao.Aguardando };

            try
            {   
                if (liberacao.StatusDesbloqueioLiberacao != StatusDesbloqueioLiberacao.Aprovado)
                {
                    var retorno = _bloqueioReferenciaAplicacao.ValidarLiberacao(liberacao.IdNotificacao, pedidoSelo.Id, Entidades.PedidoSelo, data, new Usuario { Id = UsuarioLogado.UsuarioId });
                    var modelLiberacao = new DadosValidacaoNotificacaoDesbloqueioReferenciaModal
                    {
                        IdNotificacao = retorno.Key,
                        StatusDesbloqueioLiberacao = retorno.Value,
                        IdRegistro = pedidoSelo.Id,
                        EntidadeRegistro = Entidades.PedidoSelo,
                        DataReferencia = data,
                        UsuarioLogadoId = UsuarioLogado.UsuarioId,
                        LiberacaoUtilizada = false
                    };
                    TempData[$"LiberacaoBloqueioReferencia_{Entidades.PedidoSelo}"] = modelLiberacao;
                    TempData.Keep();

                    if (retorno.Value != StatusDesbloqueioLiberacao.Aprovado)
                    {
                        divModalBloq = Helpers.RazorHelper.RenderRazorViewToString(ControllerContext, "_ModalLiberacaoBloqueioReferencia", modelLiberacao);
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
        public ActionResult SalvarDados(PedidoSeloViewModel pedidoSelo)
        {
            TempData.Keep();

            try
            {
                var liberacao = TempData.ContainsKey($"LiberacaoBloqueioReferencia_{Entidades.PedidoSelo}")
                                ? (DadosValidacaoNotificacaoDesbloqueioReferenciaModal)TempData[$"LiberacaoBloqueioReferencia_{Entidades.PedidoSelo}"]
                                : null;

                var usuarioLogadoCurrent = HttpContext.User as CustomPrincipal;
                _pedidoSeloAplicacao.Salvar(pedidoSelo, usuarioLogadoCurrent.UsuarioId);

                if (liberacao != null && liberacao.EntidadeRegistro == Entidades.PedidoSelo && liberacao.IdRegistro == pedidoSelo.Id && liberacao.IdNotificacao > 0)
                {
                    _notificacaoDesbloqueioReferenciaAplicacao.ConsumirLiberacao(liberacao.IdNotificacao, true);
                    TempData.Remove($"LiberacaoBloqueioReferencia_{Entidades.PedidoSelo}");
                }

                ModelState.Clear();

                DadosModal = new DadosModal
                {
                    Titulo = "Sucesso",
                    Mensagem = pedidoSelo.Id != 0 ? "Registro salvo com sucesso" : "Solicitação realizada com sucesso",
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
                return View("Index", pedidoSelo);
            }
            catch (BusinessRuleException br)
            {
                DadosModal = new DadosModal
                {
                    Titulo = "Atenção",
                    Mensagem = br.Message,
                    TipoModal = TipoModal.Warning
                };
                return View("Index", pedidoSelo);
            }
            catch (Exception ex)
            {
                DadosModal = new DadosModal
                {
                    Titulo = "Erro",
                    Mensagem = ex.Message,
                    TipoModal = TipoModal.Danger
                };
                return View("Index", pedidoSelo);
            }

            return View("Index", new PedidoSeloViewModel { ValidadePedido = DateTime.Now.Date.AddDays(1) });
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
                return View("Index", new PedidoSeloViewModel { ValidadePedido = DateTime.Now.Date.AddDays(1) });
            }
            catch (Exception ex)
            {
                GerarDadosModal("Atenção", new BusinessRuleException(ex.Message).Message, TipoModal.Danger);
                return View("Index", new PedidoSeloViewModel { ValidadePedido = DateTime.Now.Date.AddDays(1) });
            }
            return View("Index", new PedidoSeloViewModel { ValidadePedido = DateTime.Now.Date.AddDays(1) });
        }

        public override ActionResult ConfirmarDelete(int id)
        {
            var view = base.ConfirmarDelete(id);
            return View("Index", new PedidoSeloViewModel());
        }

        [HttpPost]
        public ActionResult ClonarPedido(int id)
        {
            TempData.Keep();

            var titulo = string.Empty;
            var mensagem = string.Empty;
            var tipo = TipoModal.Success;

            try
            {
                var usuarioLogadoCurrent = HttpContext.User as CustomPrincipal;
                _pedidoSeloAplicacao.ClonarPedido(id, usuarioLogadoCurrent.UsuarioId);
                ModelState.Clear();

                titulo = "Sucesso";
                mensagem = "Pedido clonado com sucesso";
            }
            catch (BusinessRuleException br)
            {
                titulo = "Atenção";
                mensagem = br.Message;
                tipo = TipoModal.Warning;
            }
            catch (Exception ex)
            {
                titulo = "Erro";
                mensagem = ex.Message;
                tipo = TipoModal.Danger;
            }

            return new JsonResult { Data = new { titulo, mensagem, tipo = tipo.ToDescription() }, ContentType = "application/json", MaxJsonLength = int.MaxValue };
        }

        [HttpGet, CheckSessionOut]
        public ActionResult Cancelar(int id)
        {
            GerarDadosModal("Cancelar registro", "Deseja cancelar este registro?", TipoModal.Danger, "ConfirmaCancelar",
                    "Sim, Desejo cancelar!", id);

            return View("Index", new PedidoSeloViewModel());
        }

        [HttpGet]
        public ActionResult ConfirmaCancelar(int id)
        {
            try
            {
                var usuarioLogadoCurrent = HttpContext.User as CustomPrincipal;
                _pedidoSeloAplicacao.CancelarPedido(id, usuarioLogadoCurrent.UsuarioId);

                ModelState.Clear();
                GerarDadosModal("Sucesso", "Cancelado com sucesso!", TipoModal.Success);
            }
            catch (BusinessRuleException br)
            {
                GerarDadosModal("Atenção", br.Message, TipoModal.Warning);
                return View("Index", new PedidoSeloViewModel());
            }
            catch (NotFoundException ne)
            {
                CriarModalAvisoComRetornoParaIndex(ne.Message);
            }
            catch (SoftparkIntegrationException sx)
            {
                CriarModalAvisoComRetornoParaIndex(sx.Message);
            }
            catch (Exception ex)
            {
                var message = ex.InnerException != null ? ex.InnerException.Message : ex.Message;
                GerarDadosModal("Atenção", new BusinessRuleException(message).Message, TipoModal.Danger);
                return View("Index", new PedidoSeloViewModel());
            }

            return View("Index", new PedidoSeloViewModel());
        }

        [HttpPost]
        public ActionResult CancelarPedido(int id)
        {
            TempData.Keep();

            var titulo = string.Empty;
            var mensagem = string.Empty;
            var tipo = TipoModal.Success;

            try
            {
                var usuarioLogadoCurrent = HttpContext.User as CustomPrincipal;
                _pedidoSeloAplicacao.CancelarPedido(id, usuarioLogadoCurrent.UsuarioId);
                ModelState.Clear();

                titulo = "Sucesso";
                mensagem = "Pedido cancelado com sucesso!";

                if (_pedidoSeloAplicacao.ExisteSelosAtivo(id))
                {
                    mensagem = string.Format("{0}<br />{1}", mensagem, "Porém exite(m) selo(s) gerado(s) para este pedido.");
                }
            }
            catch (BusinessRuleException br)
            {
                titulo = "Atenção";
                mensagem = br.Message;
                tipo = TipoModal.Warning;
            }
            catch (Exception ex)
            {
                titulo = "Erro";
                mensagem = ex.Message;
                tipo = TipoModal.Danger;
            }

            return new JsonResult { Data = new { titulo, mensagem, tipo = tipo.ToDescription() }, ContentType = "application/json", MaxJsonLength = int.MaxValue };
        }

        [HttpPost]
        public JsonResult BloquearLote(int id)
        {
            TempData.Keep();

            var titulo = string.Empty;
            var mensagem = string.Empty;
            var tipo = TipoModal.Success;

            try
            {
                var usuarioLogadoCurrent = HttpContext.User as CustomPrincipal;
                _pedidoSeloAplicacao.Bloquear(id, usuarioLogadoCurrent.UsuarioId);
                ModelState.Clear();

                titulo = "Sucesso";
                mensagem = "Emissão bloqueada com sucesso";
            }
            catch (BusinessRuleException br)
            {
                titulo = "Atenção";
                mensagem = br.Message;
                tipo = TipoModal.Warning;
            }
            catch (Exception ex)
            {
                titulo = "Erro";
                mensagem = ex.Message;
                tipo = TipoModal.Danger;
            }

            return new JsonResult { Data = new { titulo, mensagem, tipo = tipo.ToDescription() }, ContentType = "application/json", MaxJsonLength = int.MaxValue };
        }

        [HttpPost]
        public JsonResult DesbloquearLote(int id)
        {
            TempData.Keep();

            var titulo = string.Empty;
            var mensagem = string.Empty;
            var tipo = TipoModal.Success;

            try
            {
                var usuarioLogadoCurrent = HttpContext.User as CustomPrincipal;
                _pedidoSeloAplicacao.Desbloquear(id, usuarioLogadoCurrent.UsuarioId);
                ModelState.Clear();

                titulo = "Sucesso";
                mensagem = "Foi solicitado o desbloqueio da emissão com sucesso";
            }
            catch (BusinessRuleException br)
            {
                titulo = "Atenção";
                mensagem = br.Message;
                tipo = TipoModal.Warning;
            }
            catch (Exception ex)
            {
                titulo = "Erro";
                mensagem = ex.Message;
                tipo = TipoModal.Danger;
            }

            return new JsonResult { Data = new { titulo, mensagem, tipo = tipo.ToDescription() }, ContentType = "application/json", MaxJsonLength = int.MaxValue };
        }

        public ActionResult AtualizaGridPedidos(int statusid)
        {
            TempData.Keep();

            var pedidoSeloEnt = _pedidoSeloAplicacao.ConsultaPedidosPorStatus(statusid);
            var pedidoSeloVM = pedidoSeloEnt.Select(x => new PedidoSeloViewModel(x)).ToList();

            return PartialView("_GridPedidoSelo", pedidoSeloVM);
        }

        [HttpPost]
        [CheckSessionOut]
        public JsonResult CarregarUnidade(int idCliente)
        {
            TempData.Keep();

            var lista = _pedidoSeloAplicacao.ListaUnidadesPorCliente(idCliente).ToList();
            ListaUnidade = lista;

            var listaRetorno = lista.Select(x => new ChaveValorViewModel
            {
                Id = x.Id,
                Descricao = x.Nome
            })
            .ToList();

            TempData.Keep();

            return Json(listaRetorno, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [CheckSessionOut]
        public JsonResult CarregarConvenio(int idUnidade)
        {
            TempData.Keep();

            var lista = _pedidoSeloAplicacao.ListaConvenios(idUnidade).ToList();
            ListaConvenio = lista;

            var listaRetorno = lista.Select(x => new ChaveValorViewModel
            {
                Id = x.Id,
                Descricao = x.Descricao
            })
            .ToList();

            TempData.Keep();

            return Json(listaRetorno, JsonRequestBehavior.AllowGet);
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

        [HttpPost]
        [CheckSessionOut]
        public JsonResult CarregarProposta(int idUnidade, int idCliente)
        {
            TempData.Keep();

            var lista = _pedidoSeloAplicacao.ListaPropostas(idCliente, idUnidade).ToList();
            ListaProposta = lista;

            var listaRetorno = lista.Select(x => new ChaveValorViewModel
            {
                Id = x.Id,
                Descricao = x.Descricao
            })
            .ToList();

            TempData.Keep();

            return Json(listaRetorno, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [CheckSessionOut]
        public JsonResult CarregarTipoSelo(int idUnidade, int idConvenio)
        {
            TempData.Keep();

            var lista = _pedidoSeloAplicacao.ListaTipoSelos(idConvenio, idUnidade).ToList();
            ListaTipoSelo = lista;
                
            var listaRetorno = lista.Select(x => new ChaveValorViewModel
            {
                Id = x.Id,
                Descricao = x.Nome
            })
            .ToList();

            TempData.Keep();

            return Json(listaRetorno, JsonRequestBehavior.AllowGet);
        }

        private void PrepararTela()
        {
            TempData.Clear();

            ListaUnidade = _pedidoSeloAplicacao.ListaUnidadesPorCliente(0);
            ListaConvenio = _pedidoSeloAplicacao.ListaConvenios(0);
            ListaProposta = _pedidoSeloAplicacao.ListaPropostas(0, 0);
            ListaTipoSelo = _pedidoSeloAplicacao.ListaTipoSelos(0, 0);
            ListaNegociacaoSeloDesconto = _pedidoSeloAplicacao.ListaNegociacaoSeloDesconto();
            ListaTipoPagamento = _pedidoSeloAplicacao.ListaTipoPagamento();
            ListaStatusPedido = _pedidoSeloAplicacao.ListaStatusPedido();
            TempData.Keep();
        }

        private void PrepararTela(PedidoSelo pedidoSelo)
        {
            TempData.Clear();
            
            ListaStatusPedido = _pedidoSeloAplicacao.ListaStatusPedido();
            ListaTipoPagamento = _pedidoSeloAplicacao.ListaTipoPagamento();
            ListaConvenio = _pedidoSeloAplicacao.ListaConvenios(pedidoSelo.Unidade.Id);
            ListaTipoPedidoSelo = _pedidoSeloAplicacao.ListaTipoPedidoSelo(pedidoSelo.StatusPedido);
            ListaUnidade = _pedidoSeloAplicacao.ListaUnidadesPorCliente(pedidoSelo.Cliente.Id);
            ListaProposta = _pedidoSeloAplicacao.ListaPropostas(pedidoSelo.Cliente.Id, pedidoSelo.Unidade.Id);
            ListaTipoSelo = _pedidoSeloAplicacao.ListaTipoSelos(pedidoSelo.Convenio.Id, pedidoSelo.Unidade.Id);
            ListaNegociacaoSeloDesconto = _pedidoSeloAplicacao.ListaNegociacaoSeloDesconto();
            StatusLancamentoCobrancaPedidoSelo = _lancamentoCobrancaPedidoSeloAplicacao.RetornaStatusPorPedidoSelo(pedidoSelo.Id);

            TempData.Keep();
        }

        [HttpPost]
        [CheckSessionOut]
        public JsonResult BuscarSeloCliente(int clienteId)
        {
            var seloClienteEntity = _clienteAplicacao.BuscarSeloCliente(clienteId);
            return Json(seloClienteEntity);
        }
    }
}