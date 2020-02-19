using Aplicacao;
using Aplicacao.ViewModels;
using AutoMapper;
using Core.Extensions;
using Entidade;
using Entidade.Uteis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace Portal.Controllers
{
    public class SolicitacaoPagamentoReembolsoController : GenericController<ContasAPagar>
    {
        private readonly IPagamentoReembolsoAplicacao _pagamentoReembolsoAplicacao;
        private readonly IContaPagarAplicacao _contaPagarAplicacao;
        private readonly IContaFinanceiraAplicacao _contaFinanceiraAplicacao;
        private readonly IDepartamentoAplicacao _departamentoAplicacao;

        public SolicitacaoPagamentoReembolsoController(
            IPagamentoReembolsoAplicacao pagamentoReembolsoAplicacao,
            IContaPagarAplicacao contaPagarAplicacao,
            IContaFinanceiraAplicacao contaFinanceiraAplicacao,
            IDepartamentoAplicacao departamentoAplicacao)
        {
            _pagamentoReembolsoAplicacao = pagamentoReembolsoAplicacao;
            _contaPagarAplicacao = contaPagarAplicacao;
            _contaFinanceiraAplicacao = contaFinanceiraAplicacao;
            _departamentoAplicacao = departamentoAplicacao;
        }

        public override ActionResult Index()
        {
            CarregaViewBags();
            return View();
        }

        public ActionResult Buscar(SolicitacaoPagamentoReembolsoViewModel viewModel)
        {
            var contasAPagar = BuscarContasAPagar(viewModel);

            var model = new List<ContasAPagarViewModel>();
            model = Mapper.Map<List<ContasAPagarViewModel>>(contasAPagar);

            return PartialView("_Grid", model);
        }

        public JsonResult SalvarSolicitacoes(List<ContasAPagarViewModel> contasAPagar)
        {
            _pagamentoReembolsoAplicacao.SalvarSolicitacoes(contasAPagar);

            return Json(new
            {
                status = new HttpStatusCodeResult(HttpStatusCode.Created, "Solitações efetuadas com sucesso"),
            });
        }
        
        public ActionResult AbrirModalInformarRecibos(List<SolicitacaoPagamentoReembolsoViewModel> solicitacaoPagamentoReembolso)
        {
            var contasAPagarIds = solicitacaoPagamentoReembolso.Select(x => x.ContasAPagarId);
            var contasAPagar = _contaPagarAplicacao.BuscarPor(x => contasAPagarIds.Contains(x.Id));
            var model = new List<ContasAPagarViewModel>();
            model = Mapper.Map<List<ContasAPagarViewModel>>(contasAPagar);

            return PartialView("_ModalInformaRecibo", model);
        }
        
        private void CarregaViewBags()
        {
            ViewBag.ContaFinanceiraSelectList = new SelectList(
                _contaFinanceiraAplicacao.Buscar() ?? new List<ContaFinanceira>(),
                "Id",
                "Descricao");

            ViewBag.DepartamentoSelectList = new SelectList(
                _departamentoAplicacao.Buscar() ?? new List<Departamento>(),
                "Id",
                "Nome");

            ViewBag.ListaTipoPagamentoSelectList = new SelectList(
                Enum.GetValues(typeof(TipoContaPagamento)).Cast<TipoContaPagamento>().Select(e => new ChaveValorViewModel { Id = (int)e, Descricao = e.ToDescription() }) ?? new List<ChaveValorViewModel>(),
                "Id",
                "Descricao");
        }

        private IList<ContasAPagar> BuscarContasAPagar(SolicitacaoPagamentoReembolsoViewModel viewModel)
        {
            var contasAPagar = _contaPagarAplicacao.BuscarPor(x =>
                (viewModel.ContaFinanceiraId == null || x.ContaFinanceira.Id == viewModel.ContaFinanceiraId) &&
                (viewModel.DepartamentoId == null || x.Departamento.Id == viewModel.DepartamentoId) &&
                (viewModel.TipoPagamento == null || x.TipoPagamento == viewModel.TipoPagamento) &&
                (viewModel.DataVencimento == DateTime.MinValue || x.DataVencimento == viewModel.DataVencimento) &&
                (x.StatusConta == StatusContasAPagar.Paga)
            );

            var listaIdContasAPagar = contasAPagar.Select(x => x.Id);
            var listaIdContasAPagarRetirarGrid = _pagamentoReembolsoAplicacao
                .BuscarPor(x => listaIdContasAPagar.Contains(x.ContaAPagar.Id) && x.Status == StatusPagamentoReembolso.Pendente)
                .Select(x => x.ContaAPagar.Id)
                .ToList();

            contasAPagar = contasAPagar.Where(x => !listaIdContasAPagarRetirarGrid.Contains(x.Id)).ToList();

            return contasAPagar;
        }
    }
}