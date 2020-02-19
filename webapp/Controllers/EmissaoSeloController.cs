using Aplicacao;
using Aplicacao.ViewModels;
using Core.Exceptions;
using Core.Extensions;
using Entidade;
using Entidade.Uteis;
using Entidade.Uteis.BarCode39;
using Portal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Portal.Controllers
{
    public class EmissaoSeloController : GenericController<EmissaoSelo>
    {
        public IList<ChaveValorViewModel> ListaCliente
        {
            get { return (List<ChaveValorViewModel>)TempData["ListaClienteEmissaoSelo"] ?? new List<ChaveValorViewModel>(); }
            set { TempData["ListaClienteEmissaoSelo"] = value; }
        }

        public IList<ChaveValorViewModel> ListaConvenio
        {
            get { return (List<ChaveValorViewModel>)TempData["ListaConvenioEmissaoSelo"] ?? new List<ChaveValorViewModel>(); }
            set { TempData["ListaConvenioEmissaoSelo"] = value; }
        }

        public IList<ChaveValorViewModel> ListaUnidade
        {
            get { return (List<ChaveValorViewModel>)TempData["ListaUnidadeEmissaoSelo"] ?? new List<ChaveValorViewModel>(); }
            set { TempData["ListaUnidadeEmissaoSelo"] = value; }
        }

        public IList<ChaveValorViewModel> ListaTipoSelo
        {
            get { return (List<ChaveValorViewModel>)TempData["ListaTipoSeloEmissaoSelo"] ?? new List<ChaveValorViewModel>(); }
            set { TempData["ListaTipoSeloEmissaoSelo"] = value; }
        }

        private readonly IPrecoParametroSeloAplicacao _precoParametroSeloAplicacao;
        private readonly IPedidoSeloAplicacao _pedidoSeloAplicacao;
        private readonly ISeloAplicacao _seloAplicacao;
        private readonly IEmissaoSeloAplicacao _emissaoSeloAplicacao;
        private readonly IClienteAplicacao _clienteAplicacao;

        public EmissaoSeloController(
            IEmissaoSeloAplicacao EmissaoSeloAplicacao,
            ISeloAplicacao seloAplicacao,
            IPrecoParametroSeloAplicacao precoParametroSeloAplicacao,
            IPedidoSeloAplicacao pedidoSeloAplicacao,
            IClienteAplicacao clienteAplicacao)
        {
            _pedidoSeloAplicacao = pedidoSeloAplicacao;
            Aplicacao = EmissaoSeloAplicacao;
            _emissaoSeloAplicacao = EmissaoSeloAplicacao;
            _seloAplicacao = seloAplicacao;
            _precoParametroSeloAplicacao = precoParametroSeloAplicacao;
            _clienteAplicacao = clienteAplicacao;
        }

        public List<EmissaoSelo> ListaEmissaoSelo
        {
            get
            {
                var listaEmissao = _emissaoSeloAplicacao.ListarEmissaoSeloFiltro(new EmissaoSeloViewModel());
                return listaEmissao?.Where(x => x.StatusSelo != StatusSelo.ExcluidoLote)?.ToList() ?? new List<EmissaoSelo>();
            }
        }

        public override ActionResult Index()
        {
            PrepararTela(0, 0);
            TempData["EmissaoSeloGerado"] = null;
            return View();
        }

        [CheckSessionOut]
        [HttpPost]
        public JsonResult CancelarLote(int id)
        {
            TempData.Keep();
            var message = "";
            var tipo = TipoModal.Success;
            var divGridLotes = string.Empty;
            var divGridSelos = string.Empty;

            try
            {
                message = "Cancelamento de selos feito com sucesso!";
                var emissaoSelo = Aplicacao.BuscarPorId(id);
                emissaoSelo.StatusSelo = StatusSelo.CanceladoLote;
                _emissaoSeloAplicacao.CancelarLote(emissaoSelo);
            }
            catch (Exception ex)
            {
                message = $"Ops! Ocorreu um erro: {ex.Message}";
                tipo = TipoModal.Danger;
            }

            return new JsonResult { Data = new { message, tipo = tipo.ToDescription(), divGridSelos, divGridLotes }, ContentType = "application/json", MaxJsonLength = int.MaxValue };
        }

        [CheckSessionOut]
        public override ActionResult Delete(int id)
        {
            TempData.Keep();
            try
            {
                ModelState.Clear();

                GerarDadosModal("Remover registro", "Deseja remover este registro?", TipoModal.Danger, "ConfirmarDelete",
                    "Sim, Desejo remover!", id);
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
        public override ActionResult ConfirmarDelete(int id)
        {
            TempData.Keep();
            try
            {
                var emissao = Aplicacao.BuscarPorId(id);
                emissao.StatusSelo = StatusSelo.ExcluidoLote;
                Aplicacao.Salvar(emissao);

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
        public ActionResult SalvarDados(EmissaoSeloViewModel model)
        {
            TempData.Keep();
            try
            {
                if (model.Id == 0)
                {
                    if (TempData["EmissaoSeloGerado"] == null)
                        throw new BusinessRuleException("Não é possível gerar emissão sem Selos!");

                    var emissaoSeloGerado = (EmissaoSelo)TempData["EmissaoSeloGerado"];

                    emissaoSeloGerado.Responsavel = model.Responsavel;
                    emissaoSeloGerado.EntregaRealizada = model.EntregaRealizada;
                    emissaoSeloGerado.DataEntrega = model.DataEntrega;
                    emissaoSeloGerado.ClienteRemetente = model.ClienteRemetente;

                    _emissaoSeloAplicacao.SalvarEmissaoSeloGerada(emissaoSeloGerado);
                }
                else
                {
                    _emissaoSeloAplicacao.Editar(model);
                }

                ModelState.Clear();

                DadosModal = new DadosModal
                {
                    Titulo = "Sucesso",
                    Mensagem = "Registro salvo com sucesso!",
                    TipoModal = TipoModal.Success
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
        }

        public override ActionResult Edit(int id)
        {
            var vm = new EmissaoSeloViewModel(_emissaoSeloAplicacao.BuscarPorId(id));
            PrepararTela(vm.PedidoSelo.Convenio.Id, vm.PedidoSelo.Unidade.Id);
            return View("Index", vm);
        }

        public ActionResult FiltrarPedidoselos(PedidoSeloViewModel filtro)
        {
            TempData.Keep();
            var listaPedidoSelo = new List<PedidoSeloViewModel>();
            var pedidosEn = _pedidoSeloAplicacao.ListaPedidosSelo(filtro?.Id, filtro?.Cliente?.Id, filtro?.Convenio?.Id, filtro?.Unidade?.Id, filtro?.TipoSelo?.Id, filtro?.ValidadePedido, 0);
            listaPedidoSelo = pedidosEn.ToList();

            return PartialView("_GridPedido", listaPedidoSelo);
        }

        public ActionResult GerarSelos(EmissaoSeloViewModel filtro, PedidoSeloViewModel objPedido, DateTime? dataValidade)
        {
            TempData.Keep();
            try
            {
                ValidaParametros(filtro, objPedido);
                var emissaoSeloGerado = _emissaoSeloAplicacao.GerarEmissaoSelos(filtro.ToEntity(), objPedido.Id, dataValidade);
                var selos = emissaoSeloGerado.Selo.Select(x => new SeloViewModel(x)).ToList();

                TempData["EmissaoSeloGerado"] = emissaoSeloGerado;

                return PartialView("_GridSelo", selos);
            }
            catch (BusinessRuleException br)
            {
                return Json(new Resultado<object>()
                {
                    Sucesso = false,
                    TipoModal = TipoModal.Warning.ToDescription(),
                    Titulo = "Atenção",
                    Mensagem = br.Message,
                }, JsonRequestBehavior.AllowGet);
            }
            catch (Exception ex)
            {
                return Json(new Resultado<object>()
                {
                    Sucesso = false,
                    TipoModal = TipoModal.Danger.ToDescription(),
                    Titulo = "Atenção",
                    Mensagem = ex.Message,
                }, JsonRequestBehavior.AllowGet);
            }
        }

        public ActionResult VerificaComValidade(int idTipoSelo)
        {
            TempData.Keep();
            var possuiValidade = _emissaoSeloAplicacao.PossuiValidade(idTipoSelo);
            return Json(possuiValidade, JsonRequestBehavior.AllowGet);
        }

        public ActionResult ReiniciaSelos()
        {
            TempData.Keep();
            var listSelo = new List<SeloViewModel>();
            return PartialView("_GridSelo", listSelo);
        }

        public ActionResult Imprimir(int idemissao, string nomeImpressaoSelo)
        {
            TempData.Keep();

            var emissao = Aplicacao.BuscarPorId(idemissao);
            emissao.NomeImpressaoSelo = nomeImpressaoSelo;

            var listSelos = emissao.Selo.Select(x => new SeloViewModel(x)).ToList();

            if (listSelos == null || listSelos.Count <= 0)
            {
                return Json(new Resultado<object>()
                {
                    Sucesso = false,
                    TipoModal = TipoModal.Warning.ToDescription(),
                    Titulo = "Atenção",
                    Mensagem = "Não há selos gerados."
                }, JsonRequestBehavior.AllowGet);
            }

            listSelos.ForEach(x =>
            {
                x.UrlImagem = new BarCode().GenerateBarCode(x.Id.ToString().PadLeft(8, '0'));
                x.CodigoBarras = x.Id.ToString().PadLeft(8, '0');
            }
            );

            return View("_ImpressaoSelos", listSelos);
        }
        public ActionResult ImprimirEnvelope(int idEmissao)
        {
            TempData.Keep();

            var emissao = Aplicacao.BuscarPorId(idEmissao);
            var tabelaPrecoAvulso = _emissaoSeloAplicacao.GetTabelaPrecoAvulsoPadrao(emissao.PedidoSelo.Unidade.Id);

            if (emissao == null || emissao.Id == 0)
                return Json(new Resultado<object>()
                {
                    Sucesso = false,
                    TipoModal = TipoModal.Warning.ToDescription(),
                    Titulo = "Atenção",
                    Mensagem = "Não encontrado o registro de emissão de selo!"
                }, JsonRequestBehavior.AllowGet);

            var emissaoVM = new EmissaoSeloViewModel(emissao);
            emissaoVM.PedidoSelo.TemBoleto = emissao.Selo.Sum(x => x.Valor) > 0; //_pedidoSeloAplicacao.CalculaValorLancamentoCobranca(emissao.PedidoSelo) > 0;
            return View("_ImpressaoEnvelope", new KeyValuePair<EmissaoSeloViewModel, TabelaPrecoAvulsoViewModel>(emissaoVM, tabelaPrecoAvulso));
        }
        public ActionResult ImprimirProtocolo(int idEmissao)
        {
            TempData.Keep();

            var emissao = Aplicacao.BuscarPorId(idEmissao);
            var tabelaPrecoAvulso = _emissaoSeloAplicacao.GetTabelaPrecoAvulsoPadrao(emissao.PedidoSelo.Unidade.Id);

            if (emissao == null || emissao.Id == 0)
                return Json(new Resultado<object>()
                {
                    Sucesso = false,
                    TipoModal = TipoModal.Warning.ToDescription(),
                    Titulo = "Atenção",
                    Mensagem = "Não encontrado o registro de emissão de selo!"
                }, JsonRequestBehavior.AllowGet);

            return View("_ImpressaoProtocolo", new KeyValuePair<EmissaoSeloViewModel, TabelaPrecoAvulsoViewModel>(new EmissaoSeloViewModel(emissao), tabelaPrecoAvulso));
        }

        [HttpPost]
        [CheckSessionOut]
        public JsonResult CarregarUnidade(int idCliente)
        {
            TempData.Keep();
            var lista = _emissaoSeloAplicacao.ListaUnidadesPorCliente(idCliente);
            return Json(lista, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [CheckSessionOut]
        public JsonResult CarregarConvenio(int idUnidade)
        {
            TempData.Keep();
            var lista = _emissaoSeloAplicacao.ListaConvenio(idUnidade);
            return Json(lista, JsonRequestBehavior.AllowGet);
        }

        [CheckSessionOut]
        public JsonResult BuscarNomeConvenio(int idCliente)
        {
            var cliente = _clienteAplicacao.BuscarPorId(idCliente);
            return Json(cliente.NomeConvenio, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        [CheckSessionOut]
        public JsonResult CarregarTipoSelo(int idConvenio, int idUnidade)
        {
            TempData.Keep();
            var lista = _emissaoSeloAplicacao.ListaTipoSelo(idConvenio, idUnidade);
            return Json(lista, JsonRequestBehavior.AllowGet);
        }

        private void PrepararTela(int idConvenio = 0, int idUnidade = 0, int idCliente = 0)
        {
            TempData.Clear();
            ListaUnidade = _emissaoSeloAplicacao.ListaUnidadesPorCliente(idCliente);
            ListaConvenio = _emissaoSeloAplicacao.ListaConvenio(idUnidade);
            ListaTipoSelo = _emissaoSeloAplicacao.ListaTipoSelo(idConvenio, idUnidade);
            TempData.Keep();
        }

        private void ValidaParametros(EmissaoSeloViewModel filtro, PedidoSeloViewModel objPedido)
        {
            if (objPedido.Id == 0)
                throw new BusinessRuleException("Nenhum pedido de selo foi selecionado!");

            if (filtro.Id != 0)
                throw new BusinessRuleException("Não é Possível regerar os selos!");
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