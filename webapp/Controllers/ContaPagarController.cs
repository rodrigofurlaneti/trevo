using Aplicacao;
using Aplicacao.ViewModels;
using AutoMapper;
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
    public class ContaPagarController : GenericController<ContasAPagar>
    {
        private readonly IContaFinanceiraAplicacao _contaFinanceira;
        private readonly IDepartamentoAplicacao _departamento;
        private readonly IContaPagarAplicacao _contaPagarAplicacao;
        private readonly IFornecedorAplicacao _fornecedor;
        private readonly IUnidadeAplicacao _unidade;
        private readonly IContaContabilAplicacao _contacontabil;
        private readonly IEmpresaAplicacao _empresaAplicacao;
        private readonly IBloqueioReferenciaAplicacao _bloqueioReferenciaAplicacao;
        private readonly INotificacaoDesbloqueioReferenciaAplicacao _notificacaoDesbloqueioReferenciaAplicacao;

        public List<ContasAPagarItemViewModel> ListaItens
        {
            get => (List<ContasAPagarItemViewModel>)Session["ListaItens"] ?? new List<ContasAPagarItemViewModel>();
            set => Session["ListaItens"] = value;
        }

        public List<ContasAPagar> ListaContasPagar
        {
            get { return (List<ContasAPagar>)Session["ListaContasPagar"] ?? new List<ContasAPagar>(); }
            set { Session["ListaContasPagar"] = value; }
        }

        public ContasAPagarViewModel ContasPagarFiltro
        {
            get { return (ContasAPagarViewModel)Session["ContasPagarFiltro"] ?? new ContasAPagarViewModel(); }
            set { Session["ContasPagarFiltro"] = value; }
        }

        public List<ContaFinanceiraViewModel> ListaContaFinanceira => _contaFinanceira?.Buscar()?.Select(x => new ContaFinanceiraViewModel(x))?.ToList() ?? new List<ContaFinanceiraViewModel>();

        public List<DepartamentoViewModel> ListaDepartamento => _departamento?.Buscar()?.Select(x => new DepartamentoViewModel(x))?.ToList() ?? new List<DepartamentoViewModel>();

        public List<Fornecedor> ListaFornecedor => _fornecedor?.Buscar()?.Select(x => x)?.ToList() ?? new List<Fornecedor>();

        public List<UnidadeViewModel> ListaUnidade => _unidade?.Buscar()?.Select(x => new UnidadeViewModel(x))?.ToList() ?? new List<UnidadeViewModel>();

        public List<ContaContabilViewModel> ListaContaContabil => _contacontabil?.BuscarPor(x => x.Despesa)?.Select(x => new ContaContabilViewModel(x))?.OrderBy(x => x.Hierarquia).ToList() ?? new List<ContaContabilViewModel>();

        public IEnumerable<ChaveValorViewModel> ListaTipoJuros => Aplicacao?.BuscarValoresDoEnum<TipoJurosContaPagar>();
        public IEnumerable<ChaveValorViewModel> ListaTipoMulta => Aplicacao?.BuscarValoresDoEnum<TipoMultaContaPagar>();
        public IEnumerable<ChaveValorViewModel> ListaTipoServico => Aplicacao?.BuscarValoresDoEnum<TipoServico>();
        public IEnumerable<ChaveValorViewModel> ListaTipoPagamento => Aplicacao?.BuscarValoresDoEnum<TipoContaPagamento>();
        public IEnumerable<ChaveValorViewModel> ListaFormaPagamento => Aplicacao?.BuscarValoresDoEnum<FormaPagamento>();
        public List<ContasAPagar> ListaContasAPagar => _contaPagarAplicacao?.Buscar()?
                                                                                .Where(x => x.Ativo)?
                                                                                .OrderBy(x => x.DataVencimento)
                                                                                //.Where(x => x.Unidade != null)?
                                                                                .ToList();

        public ContaPagarController(
            IContaPagarAplicacao contaPagarAplicacao,
            IContaFinanceiraAplicacao contaFinanceira,
            IDepartamentoAplicacao departamento,
            IFornecedorAplicacao fornecedor,
            IUnidadeAplicacao unidade,
            IContaContabilAplicacao contacontabil,
            IEmpresaAplicacao empresaAplicacao,
            IBloqueioReferenciaAplicacao bloqueioReferenciaAplicacao,
            INotificacaoDesbloqueioReferenciaAplicacao notificacaoDesbloqueioReferenciaAplicacao)
        {
            Aplicacao = contaPagarAplicacao;
            _contaPagarAplicacao = contaPagarAplicacao;
            _fornecedor = fornecedor;
            _unidade = unidade;
            _contaFinanceira = contaFinanceira;
            _departamento = departamento;
            _contacontabil = contacontabil;
            _empresaAplicacao = empresaAplicacao;
            _bloqueioReferenciaAplicacao = bloqueioReferenciaAplicacao;
            _notificacaoDesbloqueioReferenciaAplicacao = notificacaoDesbloqueioReferenciaAplicacao;
        }

        [CheckSessionOut]
        public ActionResult IndexPagar()
        {
            ModelState.Clear();

            return View("IndexPagar");
        }

        public ActionResult ExecutarPagamento(int id)
        {

            var lancamentoContasPagar = new List<ContasAPagarViewModel>();

            try
            {
                ModelState.Clear();
                _contaPagarAplicacao.ExecutarPagamento(id, UsuarioLogado.UsuarioId);
                System.Threading.Thread.Sleep(1500);

                //GerarDadosModal("Executar pagamento", "Deseja realizar este pagamento?", TipoModal.Info, "ConfirmarPagamento",
                //    "Sim!", id);


                ListaContasPagar = _contaPagarAplicacao.ListarContasPagar(ContasPagarFiltro)?.ToList() ?? new List<ContasAPagar>();
                lancamentoContasPagar = ListaContasPagar?.Select(x => new ContasAPagarViewModel(x))?.ToList() ?? new List<ContasAPagarViewModel>();

            }
            catch (Exception ex)
            {
                return Json(new Resultado<object>()
                {
                    Sucesso = false,
                    TipoModal = TipoModal.Warning.ToDescription(),
                    Titulo = "Atenção",
                    Mensagem = "Ocorreu um erro ao efetivar o pagamento: " + ex.Message
                }, JsonRequestBehavior.AllowGet);
            }

            return PartialView("_GridContasPagar", lancamentoContasPagar);
        }

        public void NegarConta(int id, string observacao)
        {
            _contaPagarAplicacao.NegarConta(id, observacao, UsuarioLogado.UsuarioId);
        }

        [CheckSessionOut]
        public virtual ActionResult ConfirmarPagamento(ContasAPagarViewModel contapagar)
        {
            var lancamentoCobrancas = new List<ContasAPagarViewModel>();

            try
            {

                var objetoConta = _contaPagarAplicacao.BuscarPorId(contapagar.Id);

                objetoConta.TipoDocumentoConta = contapagar.TipoDocumentoConta;
                objetoConta.NumeroDocumento = contapagar.NumeroDocumento;

                _contaPagarAplicacao.Salvar(objetoConta, new Usuario { Id = UsuarioLogado.UsuarioId });

                _contaPagarAplicacao.ExecutarPagamento(contapagar.Id, UsuarioLogado.UsuarioId);

                ModelState.Clear();

                ListaContasPagar = _contaPagarAplicacao.ListarContasPagar(ContasPagarFiltro)?.ToList() ?? new List<ContasAPagar>();
                lancamentoCobrancas = ListaContasPagar?.Select(x => new ContasAPagarViewModel(x))?.ToList() ?? new List<ContasAPagarViewModel>();

            }
            catch (BusinessRuleException br)
            {
                ListaContasPagar = _contaPagarAplicacao.ListarContasPagar(ContasPagarFiltro)?.ToList() ?? new List<ContasAPagar>();
                lancamentoCobrancas = ListaContasPagar?.Select(x => new ContasAPagarViewModel(x))?.ToList() ?? new List<ContasAPagarViewModel>();

                GerarDadosModal("Atenção", br.Message, TipoModal.Danger);
                return PartialView("IndexPagar", lancamentoCobrancas);
            }
            catch (Exception ex)
            {
                ListaContasPagar = _contaPagarAplicacao.ListarContasPagar(ContasPagarFiltro)?.ToList() ?? new List<ContasAPagar>();
                lancamentoCobrancas = ListaContasPagar?.Select(x => new ContasAPagarViewModel(x))?.ToList() ?? new List<ContasAPagarViewModel>();

                GerarDadosModal("Atenção", new BusinessRuleException(ex.Message).Message, TipoModal.Danger);
                return PartialView("IndexPagar", lancamentoCobrancas);
            }

            ModelState.Clear();

            //return RedirectToAction("Pesquisar", new { ContasPagarFiltro });
            //return PartialView("_GridContasPagar", lancamentoCobrancas);

            return Json(new Resultado<object>()
            {
                Sucesso = false,
                TipoModal = TipoModal.Success.ToDescription(),
                Titulo = "Sucesso",
                Mensagem = "Pagamento realizado com sucesso."
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult Pesquisar(ContasAPagarViewModel filtro)
        {
            var lancamentoCobrancas = new List<ContasAPagarViewModel>();

            ContasPagarFiltro = filtro;

            try
            {
                ContasPagarFiltro = filtro;
                ListaContasPagar = _contaPagarAplicacao.ListarContasPagar(ContasPagarFiltro)?.ToList() ?? new List<ContasAPagar>();
                lancamentoCobrancas = ListaContasPagar?.Select(x => new ContasAPagarViewModel(x))?.ToList() ?? new List<ContasAPagarViewModel>();
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

            return PartialView("_GridContasPagar", lancamentoCobrancas);
        }

        public ActionResult BuscarContaPagarPeloId(int id)
        {
            try
            {
                ListaContasPagar = _contaPagarAplicacao.BuscarPor(x => x.Id == id)?.ToList();
                var contasPagar = ListaContasPagar?.Select(x => new ContasAPagarViewModel(x))?.ToList() ?? new List<ContasAPagarViewModel>();

                return PartialView("_GridContasPagar", contasPagar);
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
        }

        [CheckSessionOut]
        public override ActionResult Index()
        {
            ListaItens = new List<ContasAPagarItemViewModel>();
            ModelState.Clear();

            return View("Index");
        }

        [HttpPost]
        public JsonResult VerificacaoBloqueioReferencia(ContasAPagarViewModel model)
        {
            var divModalBloq = string.Empty;
            var data = model.DataCompetencia.HasValue && model.DataCompetencia.Value > System.Data.SqlTypes.SqlDateTime.MinValue.Value
                            ? model.DataCompetencia.Value : model.DataVencimento;
            var liberacao = TempData.ContainsKey($"LiberacaoBloqueioReferencia_{Entidades.ContasAPagar}")
                                ? (DadosValidacaoNotificacaoDesbloqueioReferenciaModal)TempData[$"LiberacaoBloqueioReferencia_{Entidades.ContasAPagar}"]
                                : new DadosValidacaoNotificacaoDesbloqueioReferenciaModal { StatusDesbloqueioLiberacao = StatusDesbloqueioLiberacao.Aguardando };

            try
            {
                if (liberacao.StatusDesbloqueioLiberacao != StatusDesbloqueioLiberacao.Aprovado)
                {
                    var retorno = _bloqueioReferenciaAplicacao.ValidarLiberacao(liberacao.IdNotificacao, model.Id, Entidades.ContasAPagar, data, new Usuario { Id = UsuarioLogado.UsuarioId });
                    var modelLiberacao = new DadosValidacaoNotificacaoDesbloqueioReferenciaModal
                    {
                        IdNotificacao = retorno.Key,
                        StatusDesbloqueioLiberacao = retorno.Value,
                        IdRegistro = model.Id,
                        EntidadeRegistro = Entidades.ContasAPagar,
                        DataReferencia = data,
                        UsuarioLogadoId = UsuarioLogado.UsuarioId,
                        LiberacaoUtilizada = false
                    };
                    TempData[$"LiberacaoBloqueioReferencia_{Entidades.ContasAPagar}"] = modelLiberacao;
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

        [CheckSessionOut]
        [HttpPost]
        public ActionResult SalvarDados(ContasAPagarViewModel model)
        {
            try
            {
                var liberacao = TempData.ContainsKey($"LiberacaoBloqueioReferencia_{Entidades.ContasAPagar}")
                                ? (DadosValidacaoNotificacaoDesbloqueioReferenciaModal)TempData[$"LiberacaoBloqueioReferencia_{Entidades.ContasAPagar}"]
                                : null;

                var listaCtFinanceira = _contaFinanceira?.Buscar();
                var listaUnidade = _unidade?.Buscar();
                var listaFornecedor = _fornecedor?.Buscar();

                model.ContaPagarItens = ListaItens;
                if (model.Id == 0)
                {
                    var valor = Convert.ToDecimal(model.ValorTotal.Replace(".", ""));
                    var codigoAgrupador = DateTime.Now.ToString("yyyyMMddHHmmss");
                    for (var i = 0; i < model.NumeroParcela; i++)
                    {
                        var entity = model.ToEntity();

                        entity.ContaFinanceira = listaCtFinanceira.FirstOrDefault(x => x.Id == model.ContaFinanceira.Id);
                        entity.Fornecedor = listaFornecedor.FirstOrDefault(x => x.Id == model.Fornecedor.Id);

                        entity.DataVencimento = model.DataVencimento.AddMonths(i);
                        entity.ValorTotal = valor / model.NumeroParcela;
                        entity.NumeroParcela = i + 1;
                        entity.CodigoAgrupadorParcela = codigoAgrupador;
                        entity.StatusConta = StatusContasAPagar.PendenteAprovacao;

                        _contaPagarAplicacao.Salvar(entity, new Usuario { Id = UsuarioLogado.UsuarioId });
                    }
                }
                else
                {
                    var entity = model.ToEntity();
                    _contaPagarAplicacao.Salvar(entity, new Usuario { Id = UsuarioLogado.UsuarioId });
                }

                if (liberacao != null && liberacao.EntidadeRegistro == Entidades.ContasAPagar && liberacao.IdRegistro == model.Id && liberacao.IdNotificacao > 0)
                {
                    _notificacaoDesbloqueioReferenciaAplicacao.ConsumirLiberacao(liberacao.IdNotificacao, true);
                    TempData.Remove($"LiberacaoBloqueioReferencia_{Entidades.ContasAPagar}");
                }

                ModelState.Clear();

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
                    Titulo = "Atenção",
                    Mensagem = ex.Message,
                    TipoModal = TipoModal.Danger
                };
                return View("Index", model);
            }

            return View("Index");
        }

        [CheckSessionOut]
        public override ActionResult Delete(int id)
        {
            var contaPagar = _contaPagarAplicacao.BuscarPorId(id);
            if (contaPagar.PossueCnab)
            {
                GerarDadosModal("Atenção", "Não é possivel deletar este registro pois já existe CNAB para ele.", TipoModal.Danger);
                return View("Index");
            }

            return base.Delete(id);
        }

        [CheckSessionOut]
        public override ActionResult ConfirmarDelete(int id)
        {
            try
            {
                _contaPagarAplicacao.ExcluirLogicamentePorId(id);

                ModelState.Clear();

                GerarDadosModal("Sucesso", "Removido com sucesso!", TipoModal.Success);
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

        public override ActionResult Edit(int id)
        {
            var entidade = _contaPagarAplicacao.BuscarPorId(id);
            var viewModel = new ContasAPagarViewModel(entidade);
            ListaItens = viewModel.ContaPagarItens;

            return View("Index", viewModel);
        }

        public ActionResult FinalizarCadastro(int id)
        {
            return this.Edit(id);
        }

        [CheckSessionOut]
        public ActionResult ExecutarPagamentoModal(int idsLancamentosCobranca)
        {

            try
            {
                var objConta = _contaPagarAplicacao.BuscarPorId(idsLancamentosCobranca);
                var ObjView = new ContasAPagarViewModel(objConta);

                return PartialView("_ModalTipoNumeroDocumento", ObjView);
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
        }

        public JsonResult BuscarEmpresaDaContaFinanceira(int contaFinanceiraId)
        {
            var empresa = _empresaAplicacao.BuscarPorId(contaFinanceiraId);
            return Json(new
            {
                empresa.Descricao
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult AdicionarItem(int contaContabilId, int unidadeId, string valor)
        {
            var unidadeVM = Mapper.Map<UnidadeViewModel>(_unidade.BuscarPorId(unidadeId));
            var contaContabil = _contacontabil.BuscarPorId(contaContabilId);
            var item = new ContasAPagarItemViewModel(contaContabil, unidadeVM, decimal.Parse(valor));
            var listaitens = ListaItens;

            listaitens.Add(item);
            ListaItens = listaitens;

            var grid = RazorHelper.RenderRazorViewToString(ControllerContext, "_GridItens", ListaItens);
            return Json(new
            {
                Grid = grid,
                ValorTotal = ListaItens.Sum(x => x.Valor)
            });
        }

        public ActionResult RemoverItem(int contaContabilId, int unidadeId, string valor)
        {
            var item = ListaItens.FirstOrDefault(x => x.ContaContabil.Id == contaContabilId && x.Unidade.Id == unidadeId && x.Valor == decimal.Parse(valor));
            var listaitens = ListaItens;

            listaitens.Remove(item);
            ListaItens = listaitens;

            var grid = RazorHelper.RenderRazorViewToString(ControllerContext, "_GridItens", ListaItens);
            return Json(new
            {
                Grid = grid,
                ValorTotal = ListaItens.Sum(x => x.Valor)
            });
        }

    }
}