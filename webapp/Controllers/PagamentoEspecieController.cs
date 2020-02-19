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
    public class PagamentoEspecieController : GenericController<ContasAPagar>
    {
        private readonly IContaPagarAplicacao _contaPagarAplicacao;
        private readonly IDepartamentoAplicacao _departamentoAplicacao;
        private readonly IRetiradaCofreAplicacao _retiradaCofreAplicacao;
        private readonly IContaFinanceiraAplicacao _contaFinanceiraAplicacao;
        private readonly IUnidadeAplicacao _unidadeAplicacao;

        public PagamentoEspecieController(
            IContaPagarAplicacao contaPagarAplicacao,
            IDepartamentoAplicacao departamentoAplicacao,
            IRetiradaCofreAplicacao retiradaCofreAplicacao,
            IContaFinanceiraAplicacao contaFinanceiraAplicacao,
            IUnidadeAplicacao unidadeAplicacao)
        {
            _contaPagarAplicacao = contaPagarAplicacao;
            _departamentoAplicacao = departamentoAplicacao;
            _retiradaCofreAplicacao = retiradaCofreAplicacao;
            _contaFinanceiraAplicacao = contaFinanceiraAplicacao;
            _unidadeAplicacao = unidadeAplicacao;
        }

        // GET: PagamentoEspecie
        public override ActionResult Index()
        {
            CarregaViewBag();

            return View();
        }

        public ActionResult BuscarPagamentoEspecie(PagamentoEspecieViewModel pagamentoEspecieViewModel)
        {
            var model = new List<ContasAPagarViewModel>();

            var contasAPagas = _contaPagarAplicacao.BuscarPor(x =>
                ((x.StatusConta == StatusContasAPagar.EmAberto || x.StatusConta == StatusContasAPagar.PendentePagamento || x.StatusConta == StatusContasAPagar.Vencida) &&
                pagamentoEspecieViewModel.DepartamentoId == null || x.Departamento.Id == pagamentoEspecieViewModel.DepartamentoId) &&
                ((x.StatusConta == StatusContasAPagar.EmAberto || x.StatusConta == StatusContasAPagar.PendentePagamento || x.StatusConta == StatusContasAPagar.Vencida) &&
                pagamentoEspecieViewModel.TipoPagamento == null || x.TipoPagamento == pagamentoEspecieViewModel.TipoPagamento) &&
                ((x.StatusConta == StatusContasAPagar.EmAberto || x.StatusConta == StatusContasAPagar.PendentePagamento || x.StatusConta == StatusContasAPagar.Vencida) &&
                pagamentoEspecieViewModel.DataVencimento == DateTime.MinValue || x.DataVencimento == pagamentoEspecieViewModel.DataVencimento) &&
                ((x.StatusConta == StatusContasAPagar.EmAberto || x.StatusConta == StatusContasAPagar.PendentePagamento || x.StatusConta == StatusContasAPagar.Vencida) &&
                pagamentoEspecieViewModel.ContaFinanceiraId == null || x.ContaFinanceira.Id == pagamentoEspecieViewModel.ContaFinanceiraId) &&
                ((x.StatusConta == StatusContasAPagar.EmAberto || x.StatusConta == StatusContasAPagar.PendentePagamento || x.StatusConta == StatusContasAPagar.Vencida) &&
                pagamentoEspecieViewModel.UnidadeId == null || x.Unidade.Id == pagamentoEspecieViewModel.UnidadeId)
            );

            model = Mapper.Map<List<ContasAPagarViewModel>>(contasAPagas);

            return PartialView("_GridPagamentoEspecie", model);
        }

        public ActionResult BuscarPagamentoEspeciePeloIdDaConta(int contaPagarId)
        {
            var contaPagar = _contaPagarAplicacao.BuscarPorId(contaPagarId);
            var contaPagarVM = Mapper.Map<ContasAPagarViewModel>(contaPagar);
            var model = new List<ContasAPagarViewModel> { contaPagarVM };

            return PartialView("_GridPagamentoEspecie", model);
        }

        public ActionResult InformarValor(List<PagamentoEspecieViewModel> pagamentoEspecie)
        {
            if (pagamentoEspecie == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Ocorreu um erro ao informar recibo: Nenhuma conta Selecionada");

            var contasAPagarIds = pagamentoEspecie.Select(x => x.ContasAPagarId);
            var contasAPagar = _contaPagarAplicacao.BuscarPor(x => contasAPagarIds.Contains(x.Id));
            var model = new List<ContasAPagarViewModel>();
            model = Mapper.Map<List<ContasAPagarViewModel>>(contasAPagar);
            
            return PartialView("_ModalInformaRecibo", model);
        }

        public ActionResult InformarObservacoes()
        {
            return PartialView("_ModalInformaObservacoes");
        }

        public JsonResult SolicitarRetirada(List<PagamentoEspecieViewModel> pagamentoEspecie, string observacoes)
        {
            _retiradaCofreAplicacao.SolicitarRetirada(pagamentoEspecie, observacoes, UsuarioLogado.UsuarioId);

            return Json(new
            {
                status = new HttpStatusCodeResult(HttpStatusCode.Created, "Valor Solicitado com sucesso"),
            });
        }

        public JsonResult SalvarRecibos(List<ContasAPagarViewModel> contasAPagar)
        {
            _contaPagarAplicacao.SalvarRecibos(contasAPagar);

            return Json(new
            {
                status = new HttpStatusCodeResult(HttpStatusCode.Created, "Recibos salvos com sucesso"),
            });
        }

        private void CarregaViewBag()
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

            ViewBag.UnidadeSelectList = new SelectList(
                _unidadeAplicacao.Buscar() ?? new List<Unidade>(),
                "Id",
                "Nome");
        }
    }
}