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
    public class PlanoCarreiraController : GenericController<PlanoCarreira>
    {
        private readonly IPlanoCarreiraAplicacao _planoCarreiraAplicacao;

        public PlanoCarreiraController(
            IPlanoCarreiraAplicacao planoCarreiraAplicacao
        )
        {
            Aplicacao = planoCarreiraAplicacao;
            _planoCarreiraAplicacao = planoCarreiraAplicacao;
        }

        [CheckSessionOut]
        public override ActionResult Index()
        {
            return View("Index");
        }

        [CheckSessionOut]
        [HttpPost]
        public ActionResult SalvarDados(PlanoCarreiraViewModel model)
        {
            try
            {
                var planoCarreira = Mapper.Map<PlanoCarreira>(model);
                _planoCarreiraAplicacao.Salvar(planoCarreira);
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
            var planoCarreira = _planoCarreiraAplicacao.BuscarPorId(id);
            var planoCarreiraVM = Mapper.Map<PlanoCarreiraViewModel>(planoCarreira);
            return View("Index", planoCarreiraVM);
        }

        public PartialViewResult BuscarPlanoCarreira()
        {
            var itens = _planoCarreiraAplicacao.Buscar();
            var itensVM = Mapper.Map<List<PlanoCarreiraViewModel>>(itens);

            return PartialView("_Grid", itensVM);
        }
    }
}