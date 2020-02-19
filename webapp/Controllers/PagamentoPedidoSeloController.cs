using Aplicacao;
using Aplicacao.ViewModels;
using Core.Exceptions;
using Entidade;
using Entidade.Uteis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using Core.Extensions;
using Portal.Models;

namespace Portal.Controllers
{
    public class PagamentoPedidoSeloController : GenericController<PedidoSelo>
    {
        private readonly IPedidoSeloAplicacao _pedidoSeloAplicacao;
        private readonly IClienteAplicacao _clienteAplicacao;
        private readonly IConvenioAplicacao _convenioAplicacao;
        private readonly IConvenioUnidadeAplicacao _convenioUnidadeAplicacao;
        private readonly IUnidadeAplicacao _unidadeAplicacao;
        private readonly ITipoSeloAplicacao _tipoSeloAplicacao;

        public IEnumerable<ChaveValorViewModel> ListaTipoPagamento;
        public List<UnidadeViewModel> ListaUnidade => AutoMapper.Mapper.Map<List<Entidade.Unidade>, List<UnidadeViewModel>>(_unidadeAplicacao.Buscar().ToList());
        public List<TipoSeloViewModel> ListaTipoSelo => AutoMapper.Mapper.Map<List<TipoSelo>, List<TipoSeloViewModel>>(_tipoSeloAplicacao.Buscar().ToList());
        public List<ClienteViewModel> ListaCliente => AutoMapper.Mapper.Map<List<Cliente>, List<ClienteViewModel>>(_clienteAplicacao.Buscar().ToList());
        public List<ConvenioViewModel> ListaConvenio => AutoMapper.Mapper.Map<List<Convenio>, List<ConvenioViewModel>>(_convenioAplicacao.Buscar().ToList());
        public List<PedidoSeloViewModel> ListaPedidoSelo => _pedidoSeloAplicacao.BuscarAprovadosPeloCliente();

        public PagamentoPedidoSeloController(IClienteAplicacao clienteAplicacao,
                                    IConvenioAplicacao convenioAplicacao,
                                    IConvenioUnidadeAplicacao convenioUnidadeAplicacao,
                                    IUnidadeAplicacao unidadeAplicacao,
                                    ITipoSeloAplicacao tipoSeloAplicacao,
                                    IPedidoSeloAplicacao pedidoSeloAplicacao)
        {
            _pedidoSeloAplicacao = pedidoSeloAplicacao;
            _clienteAplicacao = clienteAplicacao;
            _convenioAplicacao = convenioAplicacao;
            _convenioUnidadeAplicacao = convenioUnidadeAplicacao;
            _unidadeAplicacao = unidadeAplicacao;
            _tipoSeloAplicacao = tipoSeloAplicacao;

            //Tipos de pagamento
            ViewBag.ListaTipoPagamento = new SelectList(
                   Enum.GetValues(typeof(TipoPagamentoSelo)).Cast<TipoPagamentoSelo>().Select(e => new ChaveValorViewModel { Id = (int)e, Descricao = e.ToDescription() }) ?? new List<ChaveValorViewModel>(),
                   "Id",
                   "Descricao");

            ListaTipoPagamento = Enum.GetValues(typeof(TipoPagamentoSelo)).Cast<TipoPagamentoSelo>().Select(e => new ChaveValorViewModel { Id = (int)e, Descricao = e.ToDescription() });

        }

        public override ActionResult Index()
        {
            return View();
        }

        //public JsonResult BuscaUnidades(int idConvenio)
        //{
        //    //Com a unidade selecionada , busca todos clientes //CONFIRMAR COM POLLY REGRA
        //    var filter = ListaConvenio.Where(x => x.Id == idConvenio).FirstOrDefault();
        //    var ListaUnidades = new List<UnidadeViewModel>();
        //    ListaUnidades.AddRange(filter.ConvenioUnidade.Select(x => x.Unidade).Distinct());
        //    return Json(new { Unidades = ListaUnidades });
        //}

        //public JsonResult BuscaClientes(int idUnidade)
        //{
        //    //Com a unidade selecionada , busca todos clientes //CONFIRMAR COM POLLY REGRA
        //    var ListaCliente = new List<Cliente>();
        //    var filter = ListaPedidosSelo.Where(x => x.Unidade.Id == idUnidade).Distinct();
        //    ListaCliente.AddRange(filter.Select(x => x.Cliente).Distinct());
        //    return Json(new { Clientes = ListaCliente });
        //}


        //[HttpPost]
        //public ActionResult AtualizaTipoEquipes(int? unidadeid)
        //{
        //    var tipoequipes = _convenioUnidadeAplicacao.Buscar().Where(x => x.Unidade.) .BuscarPor(x => x.Unidade.Id == unidadeid).Select(x => x.).ToList();

        //    ListaTipoEquipe = tipoequipes.Select(x => new TipoEquipeViewModel(x)).ToList();

        //    JavaScriptSerializer javaScriptSerializer = new JavaScriptSerializer();
        //    string result = javaScriptSerializer.Serialize(ListaEquipe);

        //    return Json(ListaTipoEquipe, JsonRequestBehavior.AllowGet);
        //}

        public ActionResult Filtrar(PedidoSelo filtro)
        {
            var pedidosSelo = _pedidoSeloAplicacao.BuscarPedidosSeloAprovados(filtro?.Cliente?.Id, filtro?.Convenio?.Id, filtro?.Unidade?.Id, filtro?.TipoSelo?.Id, filtro?.TiposPagamento);
            return PartialView("_GridPagamentoPedidoSelo", pedidosSelo);
        }

        [CheckSessionOut]
        public ActionResult ClonarPedido(int id)
        {
            try
            {
                GerarDadosModal("Clonar registro", "Deseja clonar este pedido?", TipoModal.Info, "ConfirmarClonarPedido",
                    "Sim, Desejo clonar!", id);
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

        [CheckSessionOut]
        public ActionResult ConfirmarClonarPedido(int id)
        {
            try
            {
                var usuarioLogadoCurrent = HttpContext.User as CustomPrincipal;
                _pedidoSeloAplicacao.ClonarPedido(id, usuarioLogadoCurrent.UsuarioId);

                ModelState.Clear();
                GerarDadosModal("Sucesso", "Clonado com sucesso!", TipoModal.Success);
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

        [CheckSessionOut]
        public ActionResult CancelarPedido(int id)
        {
            try
            {
                GerarDadosModal("Cancelar registro", "Deseja cancelar este pedido?", TipoModal.Warning, "ConfirmarCancelarPedido",
                    "Sim, Desejo cancelar!", id);
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

        [CheckSessionOut]
        public ActionResult ConfirmarCancelarPedido(int id)
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
            }
            catch (Exception ex)
            {
                GerarDadosModal("Atenção", ex.Message, TipoModal.Danger);
            }

            return View("Index");
        }

        public override ActionResult Edit(int id)
        {
            var pedidoSelo = new PedidoSeloViewModel(_pedidoSeloAplicacao.BuscarPorId(id));

            return View("Index", pedidoSelo);
        }

        public ActionResult AtualizaGridPedidos(int statusid)
        {

            var pedidoSeloEnt = _pedidoSeloAplicacao.ConsultaPedidosPorStatus(statusid);

            var pedidoSeloVM = AutoMapper.Mapper.Map<List<PedidoSelo>, List<PedidoSeloViewModel>>(pedidoSeloEnt.ToList());

            return PartialView("_GridPedidoSelo", pedidoSeloVM);
        }

        protected DadosModal CriarDadosModal(string mensagem, string titulo, TipoModal tipoModal)
        {
            return new DadosModal
            {
                Titulo = titulo,
                Mensagem = mensagem,
                TipoModal = tipoModal
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
    }
}