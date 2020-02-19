using Aplicacao;
using Aplicacao.ViewModels;
using AutoMapper;
using Core.Exceptions;
using Core.Extensions;
using Entidade;
using Entidade.Uteis;
using Newtonsoft.Json;
using Portal.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web.Mvc;

namespace Portal.Controllers
{
    public class ControlePontoRHController : ControlePontoController
    {
        public override List<ControlePontoDiaViewModel> ListaControlePontoDia
        {
            get => (List<ControlePontoDiaViewModel>)Session["ListaControlePontoRHDia"];
            set => Session["ListaControlePontoRHDia"] = value;
        }

        public override List<ControlePontoUnidadeApoioViewModel> ListaControlePontoUnidadeApoio
        {
            get => (List<ControlePontoUnidadeApoioViewModel>)Session["ListaControlePontoRHUnidadeApoio"] ?? new List<ControlePontoUnidadeApoioViewModel>();
            set => Session["ListaControlePontoRHUnidadeApoio"] = value;
        }

        public ControlePontoRHController(
            IControlePontoAplicacao controlePontoAplicacao, 
            IFuncionarioAplicacao funcionarioAplicacao) : base(controlePontoAplicacao, funcionarioAplicacao)
        {
        }

        [CheckSessionOut]
        public override ActionResult Index()
        {
            ListaControlePontoDia = new List<ControlePontoDiaViewModel>();
            ListaControlePontoUnidadeApoio = new List<ControlePontoUnidadeApoioViewModel>();
            return View("Index");
        }

        [CheckSessionOut]
        public ActionResult Editar(int funcionarioId)
        {
            var controlePonto = _controlePontoAplicacao.PrimeiroPor(x => x.Funcionario.Id == funcionarioId);

            if(controlePonto == null)
            {
                controlePonto = new ControlePonto();
                controlePonto.Funcionario = _funcionarioAplicacao.BuscarPorId(funcionarioId);
            }

            var controlePontoVM = Mapper.Map<ControlePontoViewModel>(controlePonto);
            ListaControlePontoDia = _controlePontoAplicacao.RetornarControlePontoDias(controlePontoVM);
            ListaControlePontoUnidadeApoio = controlePontoVM?.ControlePontoDias?.SelectMany(x => x.UnidadesApoio).ToList();
            controlePontoVM.ControlePontoDiasDoMes = ListaControlePontoDia;

            return View("Editar", controlePontoVM);
        }

        public JsonResult AtualizarDadosTotais(int funcionarioId)
        {
            var controlePontoVM = new ControlePontoViewModel();
            controlePontoVM.Funcionario = new FuncionarioViewModel(_funcionarioAplicacao.BuscarPorId(funcionarioId));
            controlePontoVM.ControlePontoDiasDoMes = ListaControlePontoDia;

            return Json(new
            {
                controlePontoVM.IntervalosPendentes,
                controlePontoVM.TotalAdicionalNoturno,
                controlePontoVM.TotalFalta,
                controlePontoVM.TotalAtraso,
                controlePontoVM.TotalHoraExtraSessentaCinco,
                controlePontoVM.TotalHoraExtraCem,
                controlePontoVM.TotalFeriadosTrabalhados
            }, JsonRequestBehavior.AllowGet);
        }

        public JsonResult BuscarFuncionarios(BuscarGridControlePontoFuncionarioViewModel busca, int pagina = 1)
        {
            var funcionarios = new List<Funcionario>();
            PaginacaoGenericaViewModel paginacao = null;
            var quantidadePorPagina = 10;

            _controlePontoAplicacao.BuscarFuncionarios(quantidadePorPagina, ref paginacao, ref funcionarios, busca, pagina);

            ViewBag.Paginacao = paginacao;
            var partialPaginacao = RazorHelper.RenderRazorViewToString(ControllerContext, "~/Views/Shared/_PaginacaoGenericaAjax.cshtml", null);

            var funcionariosVM = funcionarios.Select(x => new FuncionarioViewModel(x)).ToList();

            ViewBag.AnoSelecionado = busca.Ano;
            ViewBag.MesSelecionado = busca.Mes;
            var grid = RazorHelper.RenderRazorViewToString(ControllerContext, "_GridFuncionariosBody", funcionariosVM);

            return Json(new
            {
                Grid = grid,
                PartialPaginacao = partialPaginacao
            });
        }
    }
}