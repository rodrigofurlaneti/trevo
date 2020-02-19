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
    public class TipoBeneficioController : GenericController<TipoBeneficio>
    {
        private readonly ITipoBeneficioAplicacao _tipoBeneficioAplicacao;

        public TipoBeneficioController(
            ITipoBeneficioAplicacao tipoBeneficioAplicacao
        )
        {
            Aplicacao = tipoBeneficioAplicacao;
            _tipoBeneficioAplicacao = tipoBeneficioAplicacao;
        }

        [CheckSessionOut]
        public override ActionResult Index()
        {
            return View("Index");
        }

        [CheckSessionOut]
        [HttpPost]
        public ActionResult SalvarDados(TipoBeneficioViewModel model)
        {
            try
            {
                var tipoBeneficio = Mapper.Map<TipoBeneficio>(model);
                _tipoBeneficioAplicacao.Salvar(tipoBeneficio);
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
            var tipoBeneficio = _tipoBeneficioAplicacao.BuscarPorId(id);
            var tipoBeneficioVM = Mapper.Map<TipoBeneficioViewModel>(tipoBeneficio);
            return View("Index", tipoBeneficioVM);
        }

        public PartialViewResult BuscarTipoBeneficio()
        {
            var tipoBeneficios = _tipoBeneficioAplicacao.Buscar();
            var tipoBeneficiosVM = Mapper.Map<List<TipoBeneficioViewModel>>(tipoBeneficios);

            return PartialView("_Grid", tipoBeneficiosVM);
        }
    }
}