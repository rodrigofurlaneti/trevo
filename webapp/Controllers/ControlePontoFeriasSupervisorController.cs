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
    public class ControlePontoFeriasSupervisorController : ControlePontoFeriasController
    {
        protected override List<ControlePontoFeriasDiaViewModel> ListaControlePontoFeriasDia
        {
            get => (List<ControlePontoFeriasDiaViewModel>)Session["ListaControlePontoFeriasSupervisorDia"];
            set => Session["ListaControlePontoFeriasSupervisorDia"] = value;
        }

        protected override List<ControlePontoFeriasUnidadeApoioViewModel> ListaControlePontoFeriasUnidadeApoio
        {
            get => (List<ControlePontoFeriasUnidadeApoioViewModel>)Session["ListaControlePontoFeriasSupervisorUnidadeApoio"] ?? new List<ControlePontoFeriasUnidadeApoioViewModel>();
            set => Session["ListaControlePontoFeriasSupervisorUnidadeApoio"] = value;
        }

        public ControlePontoFeriasSupervisorController(
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
    }
}