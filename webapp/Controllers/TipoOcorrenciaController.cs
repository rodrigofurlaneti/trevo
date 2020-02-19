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
    public class TipoOcorrenciaController : GenericController<TipoOcorrencia>
    {
        private readonly ITipoOcorrenciaAplicacao _tipoOcorrenciaAplicacao;

        public TipoOcorrenciaController(
            ITipoOcorrenciaAplicacao tipoOcorrenciaAplicacao
        )
        {
            Aplicacao = tipoOcorrenciaAplicacao;
            _tipoOcorrenciaAplicacao = tipoOcorrenciaAplicacao;
        }

        [CheckSessionOut]
        public override ActionResult Index()
        {
            return View("Index");
        }

        [CheckSessionOut]
        [HttpPost]
        public ActionResult SalvarDados(TipoOcorrenciaViewModel model)
        {
            try
            {
                var tipoOcorrencia = Mapper.Map<TipoOcorrencia>(model);
                _tipoOcorrenciaAplicacao.Salvar(tipoOcorrencia);
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
            var tipoOcorrencia = _tipoOcorrenciaAplicacao.BuscarPorId(id);
            var tipoOcorrenciaVM = Mapper.Map<TipoOcorrenciaViewModel>(tipoOcorrencia);
            return View("Index", tipoOcorrenciaVM);
        }

        public PartialViewResult BuscarTipoOcorrencia()
        {
            var itens = _tipoOcorrenciaAplicacao.Buscar();
            var itensVM = Mapper.Map<List<TipoOcorrenciaViewModel>>(itens);

            return PartialView("_Grid", itensVM);
        }

        public JsonResult BuscarPercentual(int tipoOcorrenciaId)
        {
            var tipoOcorrencia = _tipoOcorrenciaAplicacao.BuscarPorId(tipoOcorrenciaId);

            return Json(new
            {
                Percentual = tipoOcorrencia.Percentual.ToString("N2")
            }, JsonRequestBehavior.AllowGet);
        }
    }
}