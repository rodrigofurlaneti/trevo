using Aplicacao;
using Aplicacao.ViewModels;
using Entidade;
using Entidade.Uteis;
using System.Collections.Generic;
using System.Web.Mvc;

namespace Portal.Controllers
{
    public class ConsultaPagamentoPedidoController : GenericController<PedidoSelo>
    {
        public IList<ClienteViewModel> ListaCliente
        {
            get { return (List<ClienteViewModel>)TempData["ListaClienteConsultaPagamentoPedido"] ?? new List<ClienteViewModel>(); }
            set { TempData["ListaClienteConsultaPagamentoPedido"] = value; }
        }

        public IList<ConvenioViewModel> ListaConvenio
        {
            get { return (List<ConvenioViewModel>)TempData["ListaConvenioConsultaPagamentoPedido"] ?? new List<ConvenioViewModel>(); }
            set { TempData["ListaConvenioConsultaPagamentoPedido"] = value; }
        }

        public IList<UnidadeViewModel> ListaUnidade
        {
            get { return (List<UnidadeViewModel>)TempData["ListaUnidadeConsultaPagamentoPedido"] ?? new List<UnidadeViewModel>(); }
            set { TempData["ListaUnidadeConsultaPagamentoPedido"] = value; }
        }
        
        public IList<TipoSeloViewModel> ListaTipoSelo
        {
            get { return (List<TipoSeloViewModel>)TempData["ListaTipoSeloConsultaPagamentoPedido"] ?? new List<TipoSeloViewModel>(); }
            set { TempData["ListaTipoSeloConsultaPagamentoPedido"] = value; }
        }
        
        public IList<ChaveValorViewModel> ListaTipoPagamento
        {
            get { return (List<ChaveValorViewModel>)TempData["ListaTipoPagamentoConsultaPagamentoPedido"] ?? new List<ChaveValorViewModel>(); }
            set { TempData["ListaTipoPagamentoConsultaPagamentoPedido"] = value; }
        }

        private readonly IPedidoSeloAplicacao _pedidoSeloAplicacao;

        public ConsultaPagamentoPedidoController(IPedidoSeloAplicacao pedidoSeloAplicacao)
        {
            _pedidoSeloAplicacao = pedidoSeloAplicacao;
        }

        public override ActionResult Index()
        {
            PrepararTela();
            return View();
        }

        [HttpPost]
        [CheckSessionOut]
        public JsonResult CarregarGrid(int? idConvenio, int? idUnidade, int? idCliente, TipoPagamentoSelo? tipoPagamento, int? idTipoSelo)
        {
            TempData.Keep();
            //var lista = _pedidoSeloAplicacao.ListaPropostas(idCliente, idUnidade)
            //    .Select(x => new ChaveValorViewModel
            //    {
            //        Id = x.Id,
            //        Descricao = x.Descricao
            //    })
            //    .ToList();
            return null;
            //return Json(lista, JsonRequestBehavior.AllowGet);
        }

        private void PrepararTela()
        {
            TempData.Clear();
            ListaCliente = _pedidoSeloAplicacao.ListaClientes();
            ListaConvenio = _pedidoSeloAplicacao.ListaConvenios();
            ListaUnidade = _pedidoSeloAplicacao.ListaUnidades(0);
            ListaTipoSelo = _pedidoSeloAplicacao.ListaTipoSelos(0, 0);
            ListaTipoPagamento = _pedidoSeloAplicacao.ListaTipoPagamento();
            TempData.Keep();
        }
    }
}