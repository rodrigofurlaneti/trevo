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
    public class ControlePontoFeriasRHController : ControlePontoFeriasController
    {
        protected override List<ControlePontoFeriasDiaViewModel> ListaControlePontoFeriasDia
        {
            get => (List<ControlePontoFeriasDiaViewModel>)Session["ListaControlePontoFeriasRHDia"];
            set => Session["ListaControlePontoFeriasRHDia"] = value;
        }

        protected override List<ControlePontoFeriasUnidadeApoioViewModel> ListaControlePontoFeriasUnidadeApoio
        {
            get => (List<ControlePontoFeriasUnidadeApoioViewModel>)Session["ListaControlePontoFeriasRHUnidadeApoio"] ?? new List<ControlePontoFeriasUnidadeApoioViewModel>();
            set => Session["ListaControlePontoFeriasRHUnidadeApoio"] = value;
        }

        public ControlePontoFeriasRHController(
            IControlePontoFeriasAplicacao controlePontoFeriasAplicacao, 
            IFuncionarioAplicacao funcionarioAplicacao) : base(controlePontoFeriasAplicacao, funcionarioAplicacao)
        {
        }

        [CheckSessionOut]
        public override ActionResult Index()
        {
            ListaControlePontoFeriasDia = new List<ControlePontoFeriasDiaViewModel>();
            ListaControlePontoFeriasUnidadeApoio = new List<ControlePontoFeriasUnidadeApoioViewModel>();
            return View("Index");
        }

        [CheckSessionOut]
        public ActionResult Editar(int funcionarioId)
        {
            var controlePontoFerias = _controlePontoFeriasAplicacao.PrimeiroPor(x => x.Funcionario.Id == funcionarioId);

            if(controlePontoFerias == null)
            {
                controlePontoFerias = new ControlePontoFerias();
                controlePontoFerias.Funcionario = _funcionarioAplicacao.BuscarPorId(funcionarioId);
            }

            var controlePontoFeriasVM = Mapper.Map<ControlePontoFeriasViewModel>(controlePontoFerias);

            return View("Editar", controlePontoFeriasVM);
        }

        public JsonResult AtualizarDadosTotais()
        {
            var controlePontoFeriasVM = new ControlePontoFeriasViewModel();
            controlePontoFeriasVM.ControlePontoFeriasDiasDoMes = ListaControlePontoFeriasDia;

            return Json(new
            {
                controlePontoFeriasVM.IntervalosPendentes,
                controlePontoFeriasVM.TotalAdicionalNoturno,
                controlePontoFeriasVM.TotalFalta,
                controlePontoFeriasVM.TotalAtraso,
                controlePontoFeriasVM.TotalHoraExtraSessentaCinco,
                controlePontoFeriasVM.TotalHoraExtraCem,
                controlePontoFeriasVM.TotalFeriadosTrabalhados
            }, JsonRequestBehavior.AllowGet);
        }
    }
}