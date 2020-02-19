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
    public abstract class ControlePontoFeriasController : GenericController<ControlePontoFerias>
    {
        protected readonly IControlePontoFeriasAplicacao _controlePontoFeriasAplicacao;
        protected readonly IFuncionarioAplicacao _funcionarioAplicacao;

        protected abstract List<ControlePontoFeriasDiaViewModel> ListaControlePontoFeriasDia { get; set; }

        protected abstract List<ControlePontoFeriasUnidadeApoioViewModel> ListaControlePontoFeriasUnidadeApoio { get; set; }

        public ControlePontoFeriasController(
            IControlePontoFeriasAplicacao controlePontoFeriasAplicacao
            , IFuncionarioAplicacao funcionarioAplicacao
        )
        {
            Aplicacao = controlePontoFeriasAplicacao;
            _controlePontoFeriasAplicacao = controlePontoFeriasAplicacao;
            _funcionarioAplicacao = funcionarioAplicacao;

            ViewBag.Anos = RetornarAnos();
            ViewBag.Meses = RetornarMeses();
        }

        public JsonResult AtualizarGridDiasGridControlePontoFeriasUnidadeApoio(int funcionarioId, int? ano, int? mes)
        {
            var controlePontoFerias = _controlePontoFeriasAplicacao.PrimeiroPor(x => x.Funcionario.Id == funcionarioId);

            if (controlePontoFerias == null)
            {
                controlePontoFerias = new ControlePontoFerias();
                controlePontoFerias.Funcionario = _funcionarioAplicacao.BuscarPorId(funcionarioId);
            }

            var controlePontoFeriasVM = Mapper.Map<ControlePontoFeriasViewModel>(controlePontoFerias);
            ListaControlePontoFeriasDia = _controlePontoFeriasAplicacao.RetornarControlePontoFeriasDias(controlePontoFeriasVM, ano, mes);

            var listaControlePontoFeriasUnidadeApoioCombo = ListaControlePontoFeriasDia.Where(x => x.UnidadesApoio != null)?.SelectMany(x => x.UnidadesApoio)?.ToList();

            var gridDias = RazorHelper.RenderRazorViewToString(ControllerContext, "_GridDias", ListaControlePontoFeriasDia);
            var gridControlePontoFeriasUnidadeApoioCombo = RazorHelper.RenderRazorViewToString(ControllerContext, "_GridUnidadeApoioCombo", listaControlePontoFeriasUnidadeApoioCombo ?? new List<ControlePontoFeriasUnidadeApoioViewModel>());

            return Json(new
            {
                GridDias = gridDias,
                GridControlePontoFeriasUnidadeApoioCombo = gridControlePontoFeriasUnidadeApoioCombo
            });
        }

        public JsonResult BuscarFuncionarios(BuscarGridControlePontoFeriasFuncionarioViewModel busca, int pagina = 1)
        {
            var funcionarios = new List<Funcionario>();
            PaginacaoGenericaViewModel paginacao = null;
            var quantidadePorPagina = 10;

            _controlePontoFeriasAplicacao.BuscarFuncionarios(quantidadePorPagina, ref paginacao, ref funcionarios, busca, pagina);

            ViewBag.Paginacao = paginacao; 
            var partialPaginacao = RazorHelper.RenderRazorViewToString(ControllerContext, "~/Views/Shared/_PaginacaoGenericaAjax.cshtml", null);

            var funcionariosVM = funcionarios.Select(x => new FuncionarioViewModel(x)).ToList();
            var grid = RazorHelper.RenderRazorViewToString(ControllerContext, "_GridFuncionariosBody", funcionariosVM);

            return Json(new
            {
                Grid = grid,
                PartialPaginacao = partialPaginacao
            });
        }

        public JsonResult EditarControlePontoFeriasDia(DateTime data)
        {
            var controleDia = ListaControlePontoFeriasDia.FirstOrDefault(x => x.Data.Date == data.Date);
            var gridControlePontoFeriasUnidadeApoioPrincipal = RazorHelper.RenderRazorViewToString(ControllerContext, "_GridUnidadeApoioPrincipal", controleDia.UnidadesApoio ?? new List<ControlePontoFeriasUnidadeApoioViewModel>());

            return Json(new
            {
                GridControlePontoFeriasUnidadeApoioPrincipal = gridControlePontoFeriasUnidadeApoioPrincipal,
                ControleDia = controleDia
            });
        }

        public void SalvarControlePontoFeriasDia(int funcionarioId, ControlePontoFeriasDiaViewModel dto)
        {
            var listaControlePontoFeriasDia = ListaControlePontoFeriasDia;

            var controlePontoFerias = _controlePontoFeriasAplicacao.Salvar(funcionarioId, dto, listaControlePontoFeriasDia);
            var controlePontoFeriasVM = Mapper.Map<ControlePontoFeriasViewModel>(controlePontoFerias);
        }

        public JsonResult EditarControlePontoFeriasUnidadeApoio(int funcionarioId, DateTime data, int unidadeId)
        {
            var listaControlePontoFeriasDia = ListaControlePontoFeriasDia;
            var controleDia = listaControlePontoFeriasDia.FirstOrDefault(x => x.Data.Date == data.Date);

            var unidadeApoio = controleDia.UnidadesApoio.FirstOrDefault(x => x.Unidade.Id == unidadeId);
            controleDia.UnidadesApoio.Remove(unidadeApoio);

            var controlePontoFerias = _controlePontoFeriasAplicacao.PrimeiroPor(x => x.Funcionario.Id == funcionarioId);

            controlePontoFerias.ControlePontoFeriasDias = Mapper.Map<IList<ControlePontoFeriasDia>>(listaControlePontoFeriasDia);

            _controlePontoFeriasAplicacao.Salvar(controlePontoFerias);
            controlePontoFerias = _controlePontoFeriasAplicacao.BuscarPorId(controlePontoFerias.Id);
            var controlePontoFeriasVM = Mapper.Map<ControlePontoFeriasViewModel>(controlePontoFerias);

            var listaControlePontoFeriasDiaVM = Mapper.Map<List<ControlePontoFeriasDiaViewModel>>(controlePontoFerias.ControlePontoFeriasDias);
            var listaControlePontoFeriasUnidadeApoioPrincipal = listaControlePontoFeriasDiaVM.FirstOrDefault(x => x.Data.Date == data.Date).UnidadesApoio;

            var gridControlePontoFeriasUnidadeApoioPrincipal = RazorHelper.RenderRazorViewToString(ControllerContext, "_GridUnidadeApoioPrincipal", listaControlePontoFeriasUnidadeApoioPrincipal ?? new List<ControlePontoFeriasUnidadeApoioViewModel>());

            return Json(new
            {
                ControlePontoFeriasUnidadeApoio = unidadeApoio,
                GridControlePontoFeriasUnidadeApoioPrincipal = gridControlePontoFeriasUnidadeApoioPrincipal,
            });
        }

        public ActionResult SalvarControlePontoFeriasUnidadeApoio(int funcionarioId, ControlePontoFeriasUnidadeApoioViewModel dto)
        {
            var listaControlePontoFeriasDia = ListaControlePontoFeriasDia;
            var controleDia = listaControlePontoFeriasDia.FirstOrDefault(x => x.Data.Date == dto.Data.Date);

            if (controleDia.UnidadesApoio != null && controleDia.UnidadesApoio.Any(x => x.Unidade?.Id == dto.Unidade.Id))
                return new HttpStatusCodeResult(HttpStatusCode.PreconditionFailed, "Já existe um registro para essa unidade de apoio");

            controleDia.AdicionarUnidadesApoio(dto);

            var controlePontoFerias = _controlePontoFeriasAplicacao.PrimeiroPor(x => x.Funcionario.Id == funcionarioId);

            if (controlePontoFerias == null)
            {
                controlePontoFerias = new ControlePontoFerias();
                controlePontoFerias.Funcionario = _funcionarioAplicacao.BuscarPorId(funcionarioId);
            }

            controlePontoFerias.ControlePontoFeriasDias = Mapper.Map<IList<ControlePontoFeriasDia>>(listaControlePontoFeriasDia);

            _controlePontoFeriasAplicacao.Salvar(controlePontoFerias);
            controlePontoFerias = _controlePontoFeriasAplicacao.BuscarPorId(controlePontoFerias.Id);
            var controlePontoFeriasVM = Mapper.Map<ControlePontoFeriasViewModel>(controlePontoFerias);

            var listaControlePontoFeriasDiaVM = Mapper.Map<List<ControlePontoFeriasDiaViewModel>>(controlePontoFerias.ControlePontoFeriasDias);
            var listaControlePontoFeriasUnidadeApoioPrincipal = listaControlePontoFeriasDiaVM.FirstOrDefault(x => x.Data.Date == dto.Data.Date).UnidadesApoio;

            var gridControlePontoFeriasUnidadeApoioPrincipal = RazorHelper.RenderRazorViewToString(ControllerContext, "_GridUnidadeApoioPrincipal", listaControlePontoFeriasUnidadeApoioPrincipal ?? new List<ControlePontoFeriasUnidadeApoioViewModel>());

            return Json(new
            {
                GridControlePontoFeriasUnidadeApoioPrincipal = gridControlePontoFeriasUnidadeApoioPrincipal,
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
    }
}