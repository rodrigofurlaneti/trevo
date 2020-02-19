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
    public abstract class ControlePontoController : GenericController<ControlePonto>
    {
        protected readonly IControlePontoAplicacao _controlePontoAplicacao;
        protected readonly IFuncionarioAplicacao _funcionarioAplicacao;

        public abstract List<ControlePontoDiaViewModel> ListaControlePontoDia { get; set; }

        public abstract List<ControlePontoUnidadeApoioViewModel> ListaControlePontoUnidadeApoio { get; set; }

        public ControlePontoController(
            IControlePontoAplicacao controlePontoAplicacao
            , IFuncionarioAplicacao funcionarioAplicacao
        )
        {
            Aplicacao = controlePontoAplicacao;
            _controlePontoAplicacao = controlePontoAplicacao;
            _funcionarioAplicacao = funcionarioAplicacao;

            ViewBag.Anos = RetornarAnos();
            ViewBag.Meses = RetornarMeses();
        }

        public JsonResult AtualizarGridDiasGridControlePontoUnidadeApoio(int funcionarioId, int? ano, int? mes)
        {
            var controlePonto = _controlePontoAplicacao.PrimeiroPor(x => x.Funcionario.Id == funcionarioId);

            if (controlePonto == null)
            {
                controlePonto = new ControlePonto();
                controlePonto.Funcionario = _funcionarioAplicacao.BuscarPorId(funcionarioId);
            }

            var controlePontoVM = Mapper.Map<ControlePontoViewModel>(controlePonto);
            ListaControlePontoDia = _controlePontoAplicacao.RetornarControlePontoDias(controlePontoVM, ano, mes);

            var listaControlePontoUnidadeApoioCombo = ListaControlePontoDia.Where(x => x.UnidadesApoio != null)?.SelectMany(x => x.UnidadesApoio)?.ToList();

            var gridDias = RazorHelper.RenderRazorViewToString(ControllerContext, "_GridDias", ListaControlePontoDia);
            var gridControlePontoUnidadeApoioCombo = RazorHelper.RenderRazorViewToString(ControllerContext, "_GridUnidadeApoioCombo", listaControlePontoUnidadeApoioCombo ?? new List<ControlePontoUnidadeApoioViewModel>());

            return Json(new
            {
                GridDias = gridDias,
                GridControlePontoUnidadeApoioCombo = gridControlePontoUnidadeApoioCombo
            });
        }

        public JsonResult EditarControlePontoDia(DateTime data)
        {
            var controleDia = ListaControlePontoDia.FirstOrDefault(x => x.Data.Date == data.Date);
            var gridControlePontoUnidadeApoioPrincipal = RazorHelper.RenderRazorViewToString(ControllerContext, "_GridUnidadeApoioPrincipal", controleDia.UnidadesApoio ?? new List<ControlePontoUnidadeApoioViewModel>());

            return Json(new
            {
                GridControlePontoUnidadeApoioPrincipal = gridControlePontoUnidadeApoioPrincipal,
                ControleDia = controleDia
            });
        }

        public void SalvarControlePontoDia(int funcionarioId, ControlePontoDiaViewModel dto)
        {
            var listaControlePontoDia = ListaControlePontoDia;

            var controlePonto = _controlePontoAplicacao.Salvar(funcionarioId, dto, listaControlePontoDia);
            var controlePontoVM = Mapper.Map<ControlePontoViewModel>(controlePonto);
        }

        public JsonResult EditarControlePontoUnidadeApoio(int funcionarioId, DateTime data, int unidadeId)
        {
            var listaControlePontoDia = ListaControlePontoDia;
            var controleDia = listaControlePontoDia.FirstOrDefault(x => x.Data.Date == data.Date);

            var unidadeApoio = controleDia.UnidadesApoio.FirstOrDefault(x => x.Unidade.Id == unidadeId);
            controleDia.UnidadesApoio.Remove(unidadeApoio);

            var controlePonto = _controlePontoAplicacao.PrimeiroPor(x => x.Funcionario.Id == funcionarioId);

            controlePonto.ControlePontoDias = Mapper.Map<IList<ControlePontoDia>>(listaControlePontoDia);

            _controlePontoAplicacao.Salvar(controlePonto);
            controlePonto = _controlePontoAplicacao.BuscarPorId(controlePonto.Id);
            var controlePontoVM = Mapper.Map<ControlePontoViewModel>(controlePonto);

            var listaControlePontoDiaVM = Mapper.Map<List<ControlePontoDiaViewModel>>(controlePonto.ControlePontoDias);
            var listaControlePontoUnidadeApoioPrincipal = listaControlePontoDiaVM.FirstOrDefault(x => x.Data.Date == data.Date).UnidadesApoio;

            var gridControlePontoUnidadeApoioPrincipal = RazorHelper.RenderRazorViewToString(ControllerContext, "_GridUnidadeApoioPrincipal", listaControlePontoUnidadeApoioPrincipal ?? new List<ControlePontoUnidadeApoioViewModel>());

            return Json(new
            {
                ControlePontoUnidadeApoio = unidadeApoio,
                GridControlePontoUnidadeApoioPrincipal = gridControlePontoUnidadeApoioPrincipal,
            });
        }

        public ActionResult SalvarControlePontoUnidadeApoio(int funcionarioId, ControlePontoUnidadeApoioViewModel dto)
        {
            var listaControlePontoDia = ListaControlePontoDia;
            var controleDia = listaControlePontoDia.FirstOrDefault(x => x.Data.Date == dto.Data.Date);

            if (controleDia.UnidadesApoio != null && controleDia.UnidadesApoio.Any(x => x.Unidade?.Id == dto.Unidade.Id))
                return new HttpStatusCodeResult(HttpStatusCode.PreconditionFailed, "Já existe um registro para essa unidade de apoio");

            controleDia.AdicionarUnidadesApoio(dto);

            var controlePonto = _controlePontoAplicacao.PrimeiroPor(x => x.Funcionario.Id == funcionarioId);

            if (controlePonto == null)
            {
                controlePonto = new ControlePonto();
                controlePonto.Funcionario = _funcionarioAplicacao.BuscarPorId(funcionarioId);
            }

            controlePonto.ControlePontoDias = Mapper.Map<IList<ControlePontoDia>>(listaControlePontoDia);

            _controlePontoAplicacao.Salvar(controlePonto);
            controlePonto = _controlePontoAplicacao.BuscarPorId(controlePonto.Id);
            var controlePontoVM = Mapper.Map<ControlePontoViewModel>(controlePonto);

            var listaControlePontoDiaVM = Mapper.Map<List<ControlePontoDiaViewModel>>(controlePonto.ControlePontoDias);
            var listaControlePontoUnidadeApoioPrincipal = listaControlePontoDiaVM.FirstOrDefault(x => x.Data.Date == dto.Data.Date).UnidadesApoio;

            var gridControlePontoUnidadeApoioPrincipal = RazorHelper.RenderRazorViewToString(ControllerContext, "_GridUnidadeApoioPrincipal", listaControlePontoUnidadeApoioPrincipal ?? new List<ControlePontoUnidadeApoioViewModel>());

            return Json(new
            {
                GridControlePontoUnidadeApoioPrincipal = gridControlePontoUnidadeApoioPrincipal,
            });
        }

        public List<ChaveValorViewModel> RetornarAnos()
        {
            var anos = new List<ChaveValorViewModel>();

            for (int i = 2000; i < 2050; i++)
            {
                anos.Add(new ChaveValorViewModel { Id = i, Descricao = i.ToString() });
            }

            return anos;
        }

        public List<ChaveValorViewModel> RetornarMeses()
        {
            var meses = new List<ChaveValorViewModel>();

            for (int i = 1; i <= 12; i++)
            {
                var data = new DateTime(2000, i, 1);
                var nomeMes = data.ToString("MMMM").ToUpperCamelCase();
                meses.Add(new ChaveValorViewModel { Id = i, Descricao = $"{i.ToString()} - {nomeMes}" });
            }

            return meses;
        }

        public PartialViewResult BuscarFuncionariosDoSupervisor(int? supervisorId)
        {
            if (!supervisorId.HasValue)
                return PartialView("_FuncionarioAutoComplete");

            var listaFuncionario = _controlePontoAplicacao.BuscarFuncionariosDoSupervisorChaveValor(supervisorId.Value);

            return PartialView("_ListaFuncionario", listaFuncionario);
        }

        public ActionResult Impressao(int supervisorId)
        {
            var listaFuncionario = _controlePontoAplicacao.BuscarFuncionariosDoSupervisor(supervisorId);

            return View("_Impressao", listaFuncionario);
        }

        [HttpPost]
        [CheckSessionOut]
        public JsonResult BuscarDias(string dia)
        {
            return Json(ListaControlePontoDia.Where(x => x.Dia.Contains(dia)).Select(c => new
            {
                c.Dia
            }));
        }
    }
}