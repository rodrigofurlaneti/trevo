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
    public class CalendarioRHController : GenericController<CalendarioRH>
    {
        private readonly ICalendarioRHAplicacao _calendarioRHAplicacao;
        private readonly IUnidadeAplicacao _unidadeAplicacao;

        public List<CalendarioRHUnidadeViewModel> ListaCalendarioRHUnidade
        {
            get => (List<CalendarioRHUnidadeViewModel>)Session["ListaCalendarioRHUnidade"] ?? new List<CalendarioRHUnidadeViewModel>();
            set => Session["ListaCalendarioRHUnidade"] = value;
        }

        public CalendarioRHController(
            ICalendarioRHAplicacao calendarioRHAplicacao
            , IUnidadeAplicacao unidadeAplicacao
        )
        {
            Aplicacao = calendarioRHAplicacao;
            _calendarioRHAplicacao = calendarioRHAplicacao;
            _unidadeAplicacao = unidadeAplicacao;
        }

        [CheckSessionOut]
        public override ActionResult Index()
        {
            ListaCalendarioRHUnidade = new List<CalendarioRHUnidadeViewModel>();
            return View("Index");
        }

        [CheckSessionOut]
        [HttpPost]
        public ActionResult SalvarDados(CalendarioRHViewModel model)
        {
            try
            {
                model.CalendarioRHUnidades = ListaCalendarioRHUnidade;
                var calendarioRH = Mapper.Map<CalendarioRH>(model);
                _calendarioRHAplicacao.Salvar(calendarioRH);
                ModelState.Clear();
                CriarDadosModalSucesso("Registro salvo com sucesso");
            }
            catch (BusinessRuleException br)
            {
                CriarDadosModalAviso(br.Message);
                return View("Index", model);
            }
            catch (Exception ex)
            {
                CriarDadosModalErro("Ocorreu um erro ao salvar: " + ex.Message);
                return View("Index", model);
            }
            return View("Index");
        }

        public override ActionResult Edit(int id)
        {
            var calendarioRH = _calendarioRHAplicacao.BuscarPorId(id);
            var calendarioRHVM = Mapper.Map<CalendarioRHViewModel>(calendarioRH);
            ListaCalendarioRHUnidade = calendarioRHVM.CalendarioRHUnidades;
            return View("Index", calendarioRHVM);
        }

        public ActionResult AdicionarCalendarioRHUnidade(int unidadeId)
        {
            if (ListaCalendarioRHUnidade.Any(x => x.Unidade?.Id == unidadeId))
                return new HttpStatusCodeResult(HttpStatusCode.PreconditionFailed, "Já foi adicionada essa unidade");

            var unidadeVM = Mapper.Map<UnidadeViewModel>(_unidadeAplicacao.BuscarPorId(unidadeId));
            var calendarioRHUnidade = new CalendarioRHUnidadeViewModel(unidadeVM);
            var listaCalendarioRHUnidade = ListaCalendarioRHUnidade;

            listaCalendarioRHUnidade.Add(calendarioRHUnidade);
            ListaCalendarioRHUnidade = listaCalendarioRHUnidade;

            var grid = RazorHelper.RenderRazorViewToString(ControllerContext, "_GridCalendarioRHUnidade", ListaCalendarioRHUnidade);
            return Json(new
            {
                Grid = grid,
            });
        }

        public ActionResult RemoverCalendarioRHUnidade(int unidadeId)
        {
            var calendarioRHUnidade = ListaCalendarioRHUnidade.FirstOrDefault(x => x.Unidade.Id == unidadeId);
            var listaCalendarioRHUnidade = ListaCalendarioRHUnidade;

            listaCalendarioRHUnidade.Remove(calendarioRHUnidade);
            ListaCalendarioRHUnidade = listaCalendarioRHUnidade;

            var grid = RazorHelper.RenderRazorViewToString(ControllerContext, "_GridCalendarioRHUnidade", ListaCalendarioRHUnidade);
            return Json(new
            {
                Grid = grid,
            });
        }

        public ActionResult RemoverTodasCalendarioRHUnidade()
        {
            ListaCalendarioRHUnidade = new List<CalendarioRHUnidadeViewModel>();

            var grid = RazorHelper.RenderRazorViewToString(ControllerContext, "_GridCalendarioRHUnidade", ListaCalendarioRHUnidade);
            return Json(new
            {
                Grid = grid,
            });
        }

        public ActionResult EditarCalendarioRHUnidade(int unidadeId)
        {
            var calendarioRHUnidade = ListaCalendarioRHUnidade.FirstOrDefault(x => x.Unidade.Id == unidadeId);
            var listaCalendarioRHUnidade = ListaCalendarioRHUnidade;

            listaCalendarioRHUnidade.Remove(calendarioRHUnidade);
            ListaCalendarioRHUnidade = listaCalendarioRHUnidade;

            var grid = RazorHelper.RenderRazorViewToString(ControllerContext, "_GridCalendarioRHUnidade", ListaCalendarioRHUnidade);
            return Json(new
            {
                Grid = grid,
                CalendarioRHUnidade = calendarioRHUnidade
            });
        }

        public PartialViewResult BuscarCalendarioRH()
        {
            var calendarioRHs = _calendarioRHAplicacao.Buscar();
            var calendarioRHsVM = Mapper.Map<List<CalendarioRHViewModel>>(calendarioRHs);

            return PartialView("_Grid", calendarioRHsVM);
        }
    }
}