using Aplicacao;
using Aplicacao.ViewModels;
using Entidade;
using Portal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using System.ComponentModel;
using Core.Extensions;
using System.Net;
using Entidade.Uteis;
using AutoMapper;

namespace Portal.Controllers
{
    public class ConfirmacaoPagamentoReembolsoController : Controller
    {
        public List<UnidadeViewModel> Unidades
        {
            get { return (List<UnidadeViewModel>)Session["ListaUnidadeCPR"] ?? new List<UnidadeViewModel>(); }
            set { Session["ListaUnidadeCPR"] = value; }
        }
        public List<FuncionarioViewModel> Funcionarios { get; set; }
        public List<DepartamentoViewModel> Departamentos
        {
            get { return (List<DepartamentoViewModel>)Session["ListaDepartamentosCPR"] ?? new List<DepartamentoViewModel>(); }
            set { Session["ListaDepartamentosCPR"] = value; }
        }

        private readonly IUnidadeAplicacao _unidadeAplicacao;
        private readonly IFuncionarioAplicacao _funcionarioAplicacao;
        private readonly IDepartamentoAplicacao _departamentoAplicacao;
        private readonly IPagamentoReembolsoAplicacao _pagamentoReembolsoAplicacao;

        public List<PagamentoReembolso> ListaPagamentoReembolso
        {
            get { return (List<PagamentoReembolso>)Session["ListaPagamentoReembolso"] ?? new List<PagamentoReembolso>(); }
            set { Session["ListaPagamentoReembolso"] = value; }
        }

        public PagamentoReembolsoViewModel PagamentoReembolsoFiltro
        {
            get { return (PagamentoReembolsoViewModel)Session["PagamentoReembolsoFiltro"] ?? new PagamentoReembolsoViewModel(); }
            set { Session["PagamentoReembolsoFiltro"] = value; }
        }

        public ConfirmacaoPagamentoReembolsoController(IUnidadeAplicacao unidadeAplicacao
                                                      , IFuncionarioAplicacao funcionarioAplicacao
                                                      , IDepartamentoAplicacao departamentoAplicacao
                                                      , IPagamentoReembolsoAplicacao pagamentoReembolsoAplicacao)
        {
            _unidadeAplicacao = unidadeAplicacao;
            _funcionarioAplicacao = funcionarioAplicacao;
            _departamentoAplicacao = departamentoAplicacao;
            _pagamentoReembolsoAplicacao = pagamentoReembolsoAplicacao;
        }


        // GET: ConfirmacaoPagamentoReembolso
        [CheckSessionOut]
        public ActionResult Index()
        {
            Session.Remove("ListaUnidadeCPR");
            Session.Remove("ListaDepartamentosCPR");

            var dep = _departamentoAplicacao.Buscar()?.ToList() ?? new List<Departamento>();
            Departamentos = dep?.Select(x => new DepartamentoViewModel(x))?.ToList() ?? new List<DepartamentoViewModel>();
            var uni = _unidadeAplicacao.Buscar()?.ToList() ?? new List<Unidade>();
            Unidades = uni?.Select(x => new UnidadeViewModel(x))?.ToList() ?? new List<UnidadeViewModel>();


            Session["PagamentoReembolso"] = null;
            return View("Index");
        }

        public ActionResult BuscarLancamentosReembolso(PagamentoReembolsoViewModel filtro)
        {
            var pagamentosReembolso = new List<PagamentoReembolsoViewModel>();

            PagamentoReembolsoFiltro = filtro;

            try
            {
                PagamentoReembolsoFiltro = filtro;
                ListaPagamentoReembolso = _pagamentoReembolsoAplicacao.ListarLancamentoReembolso(PagamentoReembolsoFiltro)?.ToList() ?? new List<PagamentoReembolso>();
                pagamentosReembolso = ListaPagamentoReembolso?.Select(x => new PagamentoReembolsoViewModel(x))?.ToList() ?? new List<PagamentoReembolsoViewModel>();
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

            return PartialView("_GridPagamentoReembolso", pagamentosReembolso);
        }

        public ActionResult Pesquisar(PagamentoReembolsoViewModel filtro)
        {
            var pagamentosReembolso = new List<PagamentoReembolsoViewModel>();

            PagamentoReembolsoFiltro = filtro;

            try
            {
                PagamentoReembolsoFiltro = filtro;
                ListaPagamentoReembolso = _pagamentoReembolsoAplicacao.ListarLancamentoReembolso(PagamentoReembolsoFiltro)?.ToList() ?? new List<PagamentoReembolso>();
                pagamentosReembolso = ListaPagamentoReembolso?.Select(x => new PagamentoReembolsoViewModel(x))?.ToList() ?? new List<PagamentoReembolsoViewModel>();
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

            return PartialView("_GridPagamentoReembolso", pagamentosReembolso);
        }


        public ActionResult ConfirmarPagamento(List<PagamentoReembolsoViewModel> lancamentosReembolso)
        {
            if (lancamentosReembolso == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Ocorreu um erro ao realizar a ação: Nenhum pagamento de reembolso foi Selecionado");

            foreach (var item in lancamentosReembolso)
            {
                var pagamentoReembolso = _pagamentoReembolsoAplicacao.BuscarPorId(item.Id);

                //retiradaCofre.ContaAPagar.StatusConta = StatusContasAPagar.PendentePagamento;

                //_pagamentoReembolsoAplicacao.Salvar(Mapper.Map<PagamentoReembolso>(retiradaCofre.ContaAPagar));

                pagamentoReembolso.Status = StatusPagamentoReembolso.Aprovado;

                _pagamentoReembolsoAplicacao.Salvar(Mapper.Map<PagamentoReembolso>(pagamentoReembolso));
            }

            return Json(new
            {
                status = new HttpStatusCodeResult(HttpStatusCode.Created, "Pagamento de Reembolso confirmado com sucesso!"),
            });
        }

        public ActionResult NegarPagamento(List<PagamentoReembolsoViewModel> lancamentosReembolso)
        {
            if (lancamentosReembolso == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Ocorreu um erro ao realizar a ação: Nenhum pagamento de reembolso foi Selecionado");

            foreach (var item in lancamentosReembolso)
            {
                var pagamentoReembolso = _pagamentoReembolsoAplicacao.BuscarPorId(item.Id);

                //retiradaCofre.ContaAPagar.StatusConta = StatusContasAPagar.EmAberto;

                //_pagamentoReembolsoAplicacao.Salvar(Mapper.Map<PagamentoReembolso>(retiradaCofre.ContaAPagar));

                pagamentoReembolso.Status = StatusPagamentoReembolso.Negado;

                _pagamentoReembolsoAplicacao.Salvar(Mapper.Map<PagamentoReembolso>(pagamentoReembolso));
            }

            return Json(new
            {
                status = new HttpStatusCodeResult(HttpStatusCode.Created, "Valor negado com sucesso"),
            });
        }

        public ActionResult VerificaRecibo(List<PagamentoReembolsoViewModel> lancamentosReembolso)
        {
            if (lancamentosReembolso == null)
                return new HttpStatusCodeResult(HttpStatusCode.BadRequest, "Ocorreu um erro ao realizar a ação: Nenhum pagamento de reembolso foi Selecionado");

            var listaRetorno = new List<PagamentoReembolsoViewModel>();

            foreach (var item in lancamentosReembolso)
            {

                ListaPagamentoReembolso = _pagamentoReembolsoAplicacao.BuscarPor(x => x.Id == item.Id).ToList() ?? new List<PagamentoReembolso>();
                var pagamentosReembolso = ListaPagamentoReembolso?.Select(x => new PagamentoReembolsoViewModel(x))?.ToList() ?? new List<PagamentoReembolsoViewModel>();

                pagamentosReembolso.ForEach(x =>
                   listaRetorno.Add(x)
                    );
            }

            return PartialView("_ModalRecibos", listaRetorno);
        }
    }
}