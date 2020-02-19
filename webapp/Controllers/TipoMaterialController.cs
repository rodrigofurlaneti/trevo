using Aplicacao.ViewModels;
using Core.Exceptions;
using System;
using System.Web.Mvc;
using AutoMapper;
using Entidade;
using Aplicacao;
using System.Collections.Generic;

namespace Portal.Controllers
{
    public class TipoMaterialController : GenericController<TipoMaterial>
    {
        private readonly ITipoMaterialAplicacao _tipoMaterialAplicacao;

        public TipoMaterialController(ITipoMaterialAplicacao tipoMaterialAplicacao)
        {
            Aplicacao = tipoMaterialAplicacao;
            _tipoMaterialAplicacao = tipoMaterialAplicacao;
        }

        public override ActionResult Edit(int id)
        {
            var tipoMaterial = _tipoMaterialAplicacao.BuscarPorId(id);
            var tipoMaterialViewModel = Mapper.Map<TipoMaterialViewModel>(tipoMaterial);

            return View("Index", tipoMaterialViewModel);
        }

        [HttpPost]
        public ActionResult SalvarDados(TipoMaterialViewModel tipoMaterialViewModel)
        {
            try
            {
                var tipoMaterial = Mapper.Map<TipoMaterial>(tipoMaterialViewModel);
                _tipoMaterialAplicacao.Salvar(tipoMaterial);

                DadosModal = new DadosModal
                {
                    Titulo = "Sucesso",
                    Mensagem = "Registro salvo com sucesso",
                    TipoModal = TipoModal.Success
                };
            }
            catch (BusinessRuleException br)
            {
                DadosModal = new DadosModal
                {
                    Titulo = "Atenção",
                    Mensagem = br.Message,
                    TipoModal = TipoModal.Warning
                };
            }
            catch (Exception ex)
            {
                DadosModal = new DadosModal
                {
                    Titulo = "Erro",
                    Mensagem = ex.Message,
                    TipoModal = TipoModal.Danger
                };
            }

            return View("Index", tipoMaterialViewModel);
        }

        public ActionResult BuscarTiposMateriais()
        {
            var tiposMateriais = _tipoMaterialAplicacao.Buscar();

            return PartialView("_Grid", Mapper.Map<List<TipoMaterialViewModel>>(tiposMateriais));
        }
    }
}