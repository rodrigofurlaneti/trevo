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
    public class ControlePontoSupervisorController : ControlePontoController
    {
        public override List<ControlePontoDiaViewModel> ListaControlePontoDia
        {
            get => (List<ControlePontoDiaViewModel>)Session["ListaControlePontoSupervisorDia"];
            set => Session["ListaControlePontoSupervisorDia"] = value;
        }

        public override List<ControlePontoUnidadeApoioViewModel> ListaControlePontoUnidadeApoio
        {
            get => (List<ControlePontoUnidadeApoioViewModel>)Session["ListaControlePontoSupervisorUnidadeApoio"] ?? new List<ControlePontoUnidadeApoioViewModel>();
            set => Session["ListaControlePontoSupervisorUnidadeApoio"] = value;
        }

        public ControlePontoSupervisorController(
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
    }
}