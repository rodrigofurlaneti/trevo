using Aplicacao;
using Aplicacao.ViewModels;
using Core.Exceptions;
using Core.Extensions;
using Core.Helpers;
using Entidade;
using Entidade.Uteis;
using Portal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Portal.Controllers
{
    public class LancamentoCobrancaController : GenericController<LancamentoCobranca>
    {
        public List<LancamentoCobranca> ListaLancamentoCobrancas => new List<LancamentoCobranca>();
        public List<ChaveValorViewModel> ListaTipoServico => Aplicacao.BuscarValoresDoEnum<TipoServico>().ToList();
        public List<ContaFinanceiraViewModel> ListaContaFinanceira => _contaFinanceiraAplicacao.Buscar()?.Select(x => new ContaFinanceiraViewModel(x)).ToList() ?? new List<ContaFinanceiraViewModel>();
        public List<UnidadeViewModel> ListaUnidade => _unidadeAplicacao.ListarOrdenadoSimplificado()?.Select(x => new UnidadeViewModel { Id = x.Id, Codigo = x.Codigo, Nome = x.Nome })?.ToList() ?? new List<UnidadeViewModel>();

        public IEnumerable<ChaveValorViewModel> TipoValor
        {
            get
            {
                return Enum.GetValues(typeof(TipoValor)).Cast<TipoValor>().Select(e => new ChaveValorViewModel { Id = (int)e, Descricao = e.ToDescription() });
            }
        }

        public IEnumerable<ChaveValorViewModel> TipoLancamentoCobranca
        {
            get
            {
                return Enum.GetValues(typeof(StatusLancamentoCobranca)).Cast<StatusLancamentoCobranca>().Select(e => new ChaveValorViewModel { Id = (int)e, Descricao = e.ToDescription() });
            }
        }

        public List<ClienteViewModel> ListaClientes
        {
            get { return (List<ClienteViewModel>)Session["ListaClientes"] ?? new List<ClienteViewModel>(); }
            set { Session["ListaClientes"] = value; }
        }

        public List<ChaveValorCategoriaViewModel> ListaAssociados
        {
            get { return (List<ChaveValorCategoriaViewModel>)Session["ListaAssociados"] ?? new List<ChaveValorCategoriaViewModel>(); }
            set { Session["ListaAssociados"] = value; }
        }

        private readonly ILancamentoCobrancaAplicacao _lancamentoCobrancaAplicacao;
        private readonly IContaFinanceiraAplicacao _contaFinanceiraAplicacao;
        private readonly IClienteAplicacao _clienteAplicacao;
        private readonly IUnidadeAplicacao _unidadeAplicacao;
        private readonly IContratoMensalistaAplicacao _contratoMensalistaAplicacao;
        private readonly IPedidoSeloAplicacao _pedidoSeloAplicacao;
        private readonly IPedidoLocacaoAplicacao _pedidoLocacaoAplicacao;
        private readonly ILancamentoCobrancaContratoMensalistaAplicacao _lancamentoCobrancaContratoMensalistaAplicacao;
        private readonly ILancamentoCobrancaPedidoSeloAplicacao _lancamentoCobrancaPedidoSeloAplicacao;
        private readonly IPedidoLocacaoLancamentoCobrancaAplicacao _pedidoLocacaoLancamentoCobrancaAplicacao;
        private readonly IBloqueioReferenciaAplicacao _bloqueioReferenciaAplicacao;
        private readonly INotificacaoDesbloqueioReferenciaAplicacao _notificacaoDesbloqueioReferenciaAplicacao;

        public LancamentoCobrancaController(
            ILancamentoCobrancaAplicacao lancamentoCobrancaAplicacao,
            IContaFinanceiraAplicacao contaFinanceiraAplicacao,
            IClienteAplicacao clienteAplicacao,
            IUnidadeAplicacao unidadeAplicacao,
            IContratoMensalistaAplicacao contratoMensalistaAplicacao,
            IPedidoSeloAplicacao pedidoSeloAplicacao,
            IPedidoLocacaoAplicacao pedidoLocacaoAplicacao,
            ILancamentoCobrancaContratoMensalistaAplicacao lancamentoCobrancaContratoMensalistaAplicacao,
            ILancamentoCobrancaPedidoSeloAplicacao lancamentoCobrancaPedidoSeloAplicacao,
            IPedidoLocacaoLancamentoCobrancaAplicacao pedidoLocacaoLancamentoCobrancaAplicacao,
            IBloqueioReferenciaAplicacao bloqueioReferenciaAplicacao,
            INotificacaoDesbloqueioReferenciaAplicacao notificacaoDesbloqueioReferenciaAplicacao)
        {
            Aplicacao = lancamentoCobrancaAplicacao;
            _lancamentoCobrancaAplicacao = lancamentoCobrancaAplicacao;
            _contaFinanceiraAplicacao = contaFinanceiraAplicacao;
            _clienteAplicacao = clienteAplicacao;
            _unidadeAplicacao = unidadeAplicacao;
            _contratoMensalistaAplicacao = contratoMensalistaAplicacao;
            _pedidoSeloAplicacao = pedidoSeloAplicacao;
            _pedidoLocacaoAplicacao = pedidoLocacaoAplicacao;
            _lancamentoCobrancaContratoMensalistaAplicacao = lancamentoCobrancaContratoMensalistaAplicacao;
            _lancamentoCobrancaPedidoSeloAplicacao = lancamentoCobrancaPedidoSeloAplicacao;
            _pedidoLocacaoLancamentoCobrancaAplicacao = pedidoLocacaoLancamentoCobrancaAplicacao;
            _bloqueioReferenciaAplicacao = bloqueioReferenciaAplicacao;
            _notificacaoDesbloqueioReferenciaAplicacao = notificacaoDesbloqueioReferenciaAplicacao;
        }

        [CheckSessionOut]
        public override ActionResult Index()
        {
            ModelState.Clear();
            Session["ListaAssociados"] = null;

            return View("Index");
        }

        [HttpPost]
        public JsonResult VerificacaoBloqueioReferencia(LancamentoCobrancaViewModel model)
        {
            var divModalBloq = string.Empty;
            var data = model.DataCompetencia.HasValue && model.DataCompetencia.Value > System.Data.SqlTypes.SqlDateTime.MinValue.Value
                            ? model.DataCompetencia.Value : model.DataVencimento;
            var liberacao = TempData.ContainsKey($"LiberacaoBloqueioReferencia_{Entidades.LancamentoCobranca}")
                                ? (DadosValidacaoNotificacaoDesbloqueioReferenciaModal)TempData[$"LiberacaoBloqueioReferencia_{Entidades.LancamentoCobranca}"]
                                : new DadosValidacaoNotificacaoDesbloqueioReferenciaModal { StatusDesbloqueioLiberacao = StatusDesbloqueioLiberacao.Aguardando };
            try
            {
                if (liberacao.StatusDesbloqueioLiberacao != StatusDesbloqueioLiberacao.Aprovado)
                {
                    var retorno = _bloqueioReferenciaAplicacao.ValidarLiberacao(liberacao.IdNotificacao, model.Id, Entidades.LancamentoCobranca, data, new Usuario { Id = UsuarioLogado.UsuarioId });
                    var modelLiberacao = new DadosValidacaoNotificacaoDesbloqueioReferenciaModal
                    {
                        IdNotificacao = retorno.Key,
                        StatusDesbloqueioLiberacao = retorno.Value,
                        IdRegistro = model.Id,
                        EntidadeRegistro = Entidades.LancamentoCobranca,
                        DataReferencia = data,
                        UsuarioLogadoId = UsuarioLogado.UsuarioId,
                        LiberacaoUtilizada = false
                    };
                    TempData[$"LiberacaoBloqueioReferencia_{Entidades.LancamentoCobranca}"] = modelLiberacao;
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

        [CheckSessionOut]
        [HttpPost]
        public ActionResult SalvarDados(LancamentoCobrancaViewModel model)
        {
            try
            {
                var liberacao = TempData.ContainsKey($"LiberacaoBloqueioReferencia_{Entidades.LancamentoCobranca}")
                                ? (DadosValidacaoNotificacaoDesbloqueioReferenciaModal)TempData[$"LiberacaoBloqueioReferencia_{Entidades.LancamentoCobranca}"]
                                : null;

                var entity = AutoMapper.Mapper.Map<LancamentoCobrancaViewModel, LancamentoCobranca>(model);

                entity.DataInsercao = DateTime.Now;
                entity.DataGeracao = DateTime.Now;

                var tipoServicoBase = model.Id > 0 ? _lancamentoCobrancaAplicacao.BuscarPorId(model.Id)?.TipoServico ?? new TipoServico?() : model.TipoServico;

                entity = _lancamentoCobrancaAplicacao.SalvarComRetorno(entity);
                model.Id = entity.Id;

                if (liberacao != null && liberacao.EntidadeRegistro == Entidades.LancamentoCobranca && liberacao.IdRegistro == model.Id && liberacao.IdNotificacao > 0)
                {
                    _notificacaoDesbloqueioReferenciaAplicacao.ConsumirLiberacao(liberacao.IdNotificacao, true);
                    TempData.Remove($"LiberacaoBloqueioReferencia_{Entidades.LancamentoCobranca}");
                }

                model.ListaCobrancaTipoServico = ListaAssociados;
                RemoverCobrancaTipoServico(tipoServicoBase, model);
                AdicionarCobrancaTipoServico(entity, model);

                ModelState.Clear();
                Session["ListaAssociados"] = null;

                DadosModal = new DadosModal
                {
                    Titulo = "Sucesso",
                    Mensagem = "Registro salvo com sucesso",
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
                return View("Index", model);
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

        public ActionResult AtualizarAssociados(List<ChaveValorCategoriaViewModel> associados)
        {
            var lista = Session["ListaAssociados"] == null ? new List<ChaveValorCategoriaViewModel>() : (List<ChaveValorCategoriaViewModel>)Session["ListaAssociados"];

            if (associados != null)
            {
                foreach (var item in associados)
                {
                    if (Convert.ToInt32(item.Id) > 0 && ListaAssociados.All(x => x.Id != item.Id))
                    {
                        item.Categoria = ((TipoServico)Convert.ToInt32(item.Categoria)).ToDescription();
                        lista.Add(item);
                    }
                }
            }

            Session["ListaAssociados"] = lista;
            return PartialView("_GridAssociados", ListaAssociados);
        }
        public ActionResult RemoverAssociado(string id)
        {
            var lista = Session["ListaAssociados"] == null ? new List<ChaveValorCategoriaViewModel>() : (List<ChaveValorCategoriaViewModel>)Session["ListaAssociados"];
            if (lista.Any(x => x.Id == id))
                lista.Remove(lista.FirstOrDefault(x => x.Id == id));
            Session["ListaAssociados"] = lista;
            return PartialView("_GridAssociados", ListaAssociados);
        }

        private void AdicionarCobrancaTipoServico(LancamentoCobranca entity, LancamentoCobrancaViewModel model)
        {
            switch (model.TipoServico)
            {
                case TipoServico.Mensalista:
                    var lista = _lancamentoCobrancaContratoMensalistaAplicacao.BuscarPor(x => x.Id == model.Id);
                    foreach (var item in model.ListaCobrancaTipoServico.Where(x => x.Categoria == TipoServico.Mensalista.ToDescription()))
                    {
                        if (lista.All(x => x.Id != Convert.ToInt32(item.Id)))
                        {
                            var mensal = new LancamentoCobrancaContratoMensalista
                            {
                                LancamentoCobranca = new LancamentoCobranca { Id = entity.Id },
                                ContratoMensalista = new ContratoMensalista { Id = Convert.ToInt32(item.Id) }
                            };
                            _lancamentoCobrancaContratoMensalistaAplicacao.Salvar(mensal);
                        }
                    }
                    break;
                //case TipoServico.Convenio:
                //    _lancamentoCobrancaPedidoSeloAplicacao.Salvar(new LancamentoCobrancaPedidoSelo { Id = entity.Id, PedidoSelo = new PedidoSelo { Id = Convert.ToInt32(model.CobrancaTipoServico.Id) } });
                //    break;
                //case TipoServico.Locacao:
                //    _pedidoLocacaoLancamentoCobrancaAplicacao.Salvar(new PedidoLocacaoLancamentoCobranca { PedidoLocacao = new PedidoLocacao { Id = Convert.ToInt32(model.CobrancaTipoServico.Id) }, LancamentoCobranca = entity });
                //    break;
                default:
                    break;
                    //throw new NotImplementedException($"Não implementado para este tipo de serviço [{model.TipoServico.ToDescription()}]");
            }
        }
        private void RemoverCobrancaTipoServico(TipoServico? tipoServicoBase, LancamentoCobrancaViewModel model)
        {
            switch (tipoServicoBase.Value)
            {
                case TipoServico.Mensalista:
                    var lista = _lancamentoCobrancaContratoMensalistaAplicacao.BuscarPor(x => x.Id == model.Id);
                    if (lista != null && lista.Any())
                    {
                        foreach (var item in lista)
                        {
                            if (model.ListaCobrancaTipoServico
                                        .Where(x => x.Categoria == TipoServico.Mensalista.ToDescription())
                                        .All(x => Convert.ToInt32(x.Id) != item.Id))
                            {
                                _lancamentoCobrancaContratoMensalistaAplicacao.Excluir(item);
                            }
                        }
                    }
                    break;
                //case TipoServico.Convenio:
                //    var selo = _lancamentoCobrancaPedidoSeloAplicacao.BuscarPor(x => x.Id == model.Id)?.FirstOrDefault();
                //    if (selo != null && selo.Id > 0)
                //        _lancamentoCobrancaPedidoSeloAplicacao.Excluir(selo);
                //    break;
                //case TipoServico.Locacao:
                //    var locacao = _pedidoLocacaoLancamentoCobrancaAplicacao.BuscarPor(x => x.Id == model.Id)?.FirstOrDefault();
                //    if (locacao != null && locacao.Id > 0)
                //        _pedidoLocacaoLancamentoCobrancaAplicacao.Excluir(locacao);
                //    break;
                default:
                    var cobMensal = _lancamentoCobrancaContratoMensalistaAplicacao.BuscarPor(x => x.Id == model.Id)?.FirstOrDefault();
                    if (cobMensal != null && cobMensal.Id > 0)
                        _lancamentoCobrancaContratoMensalistaAplicacao.Excluir(cobMensal);
                    //var cobSelo = _lancamentoCobrancaPedidoSeloAplicacao.BuscarPor(x => x.Id == model.Id)?.FirstOrDefault();
                    //if (cobSelo != null && cobSelo.Id > 0)
                    //    _lancamentoCobrancaPedidoSeloAplicacao.Excluir(cobSelo);
                    //var cobLocacao = _pedidoLocacaoLancamentoCobrancaAplicacao.BuscarPor(x => x.Id == model.Id)?.FirstOrDefault();
                    //if (cobLocacao != null && cobLocacao.Id > 0)
                    //    _pedidoLocacaoLancamentoCobrancaAplicacao.Excluir(cobLocacao);
                    break;
            }
        }

        public override ActionResult Delete(int id)
        {
            var lancamentoCobranca = _lancamentoCobrancaAplicacao.BuscarPorId(id);
            if (lancamentoCobranca.PossueCnab)
            {
                GerarDadosModal("Atenção", "Não é possivel deletar este registro pois já existe CNAB para ele.", TipoModal.Danger);
                return View("Index");
            }

            return base.Delete(id);
        }

        [CheckSessionOut]
        public override ActionResult Edit(int id)
        {
            Session["ListaAssociados"] = null;

            var lancamento = Aplicacao.BuscarPorId(id);
            var item = new LancamentoCobrancaViewModel(lancamento);
            switch (item.TipoServico)
            {
                case TipoServico.Mensalista:
                    var lista = _lancamentoCobrancaContratoMensalistaAplicacao.BuscarPor(x => x.LancamentoCobranca.Id == item.Id)?.Select(x => x.ContratoMensalista)?.ToList() ?? new List<ContratoMensalista>();
                    //item.CobrancaTipoServico = new ChaveValorCategoriaViewModel(mensal.Id.ToString(), mensal.NumeroContrato.ToString(), TipoServico.Mensalista.ToDescription());
                    item.ListaCobrancaTipoServico = lista != null
                                                    ? lista.Select(x => new ChaveValorCategoriaViewModel(x.Id.ToString(), x.NumeroContrato.ToString(), TipoServico.Mensalista.ToDescription())).ToList()
                                                    : new List<ChaveValorCategoriaViewModel>();
                    break;
                //case TipoServico.Convenio:
                //    var convenio = _lancamentoCobrancaPedidoSeloAplicacao.BuscarPor(x => x.Id == item.Id)?.FirstOrDefault()?.PedidoSelo;
                //    item.CobrancaTipoServico = new ChaveValorCategoriaViewModel(convenio.Id.ToString(), convenio.Quantidade.ToString(), TipoServico.Convenio.ToDescription());
                //    break;
                //case TipoServico.Locacao:
                //    var locacao = _pedidoLocacaoLancamentoCobrancaAplicacao.BuscarPor(x => x.LancamentoCobranca.Id == item.Id)?.FirstOrDefault()?.PedidoLocacao;
                //    item.CobrancaTipoServico = new ChaveValorCategoriaViewModel(locacao.Id.ToString(), locacao.Valor.ToString("C2"), TipoServico.Locacao.ToDescription());
                //    break;
                default:
                    break;
            }
            Session["ListaAssociados"] = item.ListaCobrancaTipoServico;

            return View("Index", item);
        }

        [CheckSessionOut]
        public ActionResult BuscarLancamentoCobrancas(int status, string unidade, string cliente, string dataVencimento, string contrato)
        {
            var lancamentosCobranca = new List<LancamentoCobrancaViewModel>();
            var grid = string.Empty;

            try
            {
                lancamentosCobranca = _lancamentoCobrancaAplicacao.BuscarLancamentosCobranca(status == 0 ? new StatusLancamentoCobranca?() : (StatusLancamentoCobranca)status,
                                                                                                string.IsNullOrEmpty(unidade) ? 0 : Convert.ToInt32(unidade),
                                                                                                string.IsNullOrEmpty(cliente) ? 0 : Convert.ToInt32(cliente),
                                                                                                dataVencimento,
                                                                                                string.IsNullOrEmpty(contrato) ? 0 : Convert.ToInt32(contrato))?
                                        .Select(x => new LancamentoCobrancaViewModel(x))?
                                        .ToList() ?? new List<LancamentoCobrancaViewModel>();

                grid = RazorHelper.RenderRazorViewToString(ControllerContext, "_GridLancamentoCobranca", lancamentosCobranca);
            }
            catch (Exception ex)
            {
                return Json(new Resultado<object>()
                {
                    Sucesso = false,
                    TipoModal = TipoModal.Warning.ToDescription(),
                    Titulo = "Atenção",
                    Mensagem = "Ocorreu um erro ao buscar lançamentos cobrança: " + ex.Message
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
        public JsonResult BuscarEntidadePorTipoServico(string descricao, string tipoServico, string unidade, string cliente)
        {
            var lista = new List<ChaveValorViewModel>();
            var servico = (TipoServico)Convert.ToInt32(tipoServico);
            switch (servico)
            {
                case TipoServico.Mensalista:
                    lista = _contratoMensalistaAplicacao.BuscarPor(x => x.NumeroContrato.ToString().Contains(descricao)
                                                                        && x.Cliente.Id == Convert.ToInt32(cliente))?
                                .Select(x => new ChaveValorViewModel(x.Id, $"Nº:[{x.NumeroContrato.ToString()}]"))?.ToList();
                    break;
                default:
                    break;
            }
            return Json(lista.ToList());
        }

        public JsonResult BuscarEmpresa(int contaFinanceiraId, int unidadeId)
        {
            var unidade = _unidadeAplicacao.BuscarPorId(unidadeId);
            var contaFinanceira = unidade?.Empresa != null ? _contaFinanceiraAplicacao.PrimeiroPor(x => x.Id == contaFinanceiraId && x.Empresa.Id == unidade.Empresa.Id) : null;
            return Json(contaFinanceira?.Empresa?.RazaoSocial ?? string.Empty, JsonRequestBehavior.AllowGet);
        }

        public JsonResult PreencheCamposContrato(int id)
        {
            var contrato = _contratoMensalistaAplicacao.BuscarPorId(id);

            return new JsonResult
            {
                Data = new
                {
                    DataVencimento = contrato.DataVencimento.ToShortDateString(),
                    ValorContrato = contrato.Valor.ToString("N2"),
                    Unidade = contrato.Unidade.Nome,
                    Cliente = string.IsNullOrEmpty(contrato.Cliente.NomeFantasia) ? contrato.Cliente.Pessoa.Nome : contrato.Cliente.NomeFantasia
                },
                ContentType = "application/json",
                MaxJsonLength = int.MaxValue
            };
        }
    }
}