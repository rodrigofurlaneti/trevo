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
    public class BeneficioFuncionarioController : GenericController<BeneficioFuncionario>
    {
        private readonly IBeneficioFuncionarioAplicacao _beneficioFuncionarioAplicacao;
        private readonly ITipoBeneficioAplicacao _tipoBeneficioAplicacao;
        private readonly IPlanoCarreiraAplicacao _planoCarreiraAplicacao;
        private readonly IFuncionarioAplicacao _funcionarioAplicacao;

        public string GuidSession;

        public List<BeneficioFuncionarioDetalheViewModel> ListaBeneficioFuncionarioDetalhes
        {
            get
            {
                return (List<BeneficioFuncionarioDetalheViewModel>)TempData[$"ListaBeneficioItemDetalhes_{GuidSession}"] ?? new List<BeneficioFuncionarioDetalheViewModel>();
            }
            set
            {

                TempData[$"ListaBeneficioItemDetalhes_{GuidSession}"] = value;
                TempData.Keep($"ListaBeneficioItemDetalhes_{GuidSession}");
            }
        }

        public BeneficioFuncionarioController(
            IBeneficioFuncionarioAplicacao beneficioFuncionarioAplicacao
            , ITipoBeneficioAplicacao tipoBeneficioAplicacao
            , IPlanoCarreiraAplicacao planoCarreiraAplicacao
            , IFuncionarioAplicacao funcionarioAplicacao
        )
        {
            Aplicacao = beneficioFuncionarioAplicacao;
            _beneficioFuncionarioAplicacao = beneficioFuncionarioAplicacao;
            _tipoBeneficioAplicacao = tipoBeneficioAplicacao;
            _planoCarreiraAplicacao = planoCarreiraAplicacao;
            _funcionarioAplicacao = funcionarioAplicacao;

            ViewBag.ListaTipoBeneficio = _tipoBeneficioAplicacao.BuscarPor(x => x.Ativo);
        }

        [CheckSessionOut]
        public override ActionResult Index()
        {
            GuidSession = string.Empty;
            ListaBeneficioFuncionarioDetalhes = new List<BeneficioFuncionarioDetalheViewModel>();
            return View("Index");
        }

        [CheckSessionOut]
        [HttpPost]
        public ActionResult SalvarDados(string guidSession, BeneficioFuncionarioViewModel model)
        {
            try
            {
                GuidSession = guidSession;
                model.BeneficioFuncionarioDetalhes = ListaBeneficioFuncionarioDetalhes;
                var beneficioFuncionario = Mapper.Map<BeneficioFuncionario>(model);
                _beneficioFuncionarioAplicacao.Salvar(beneficioFuncionario);
                TempData.Remove($"ListaBeneficioItemDetalhes_{GuidSession}");
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
            return null;
        }

        public ActionResult Edicao(int id, string guidSession)
        {
            GuidSession = guidSession;
            var beneficioFuncionario = _beneficioFuncionarioAplicacao.BuscarPorId(id);
            var beneficioFuncionarioVM = Mapper.Map<BeneficioFuncionarioViewModel>(beneficioFuncionario);

            if (beneficioFuncionarioVM.BeneficioFuncionarioDetalhes != null)
            {
                foreach (var item in beneficioFuncionarioVM.BeneficioFuncionarioDetalhes)
                {
                    item.Valor = _planoCarreiraAplicacao.BuscarValorPeloPeriodo(item.TipoBeneficio, beneficioFuncionarioVM.Funcionario.Id);
                }
            }

            ListaBeneficioFuncionarioDetalhes = beneficioFuncionarioVM.BeneficioFuncionarioDetalhes;
            return View("Index", beneficioFuncionarioVM);
        }

        public PartialViewResult BuscarBeneficioFuncionario()
        {
            var beneficioFuncionarios = _beneficioFuncionarioAplicacao.BuscarPor(x => x.BeneficioFuncionarioDetalhes.Any());
            var beneficioFuncionariosVM = Mapper.Map<List<BeneficioFuncionarioViewModel>>(beneficioFuncionarios);

            return PartialView("_Grid", beneficioFuncionariosVM);
        }

        public ActionResult AdicionarBeneficioFuncionario(int funcionarioId, int tipoBeneficioId, string valor, string guidSession)
        {
            GuidSession = guidSession;
            if (ListaBeneficioFuncionarioDetalhes.Any(x => x.TipoBeneficio.Id == tipoBeneficioId))
                return new HttpStatusCodeResult(HttpStatusCode.PreconditionFailed, "Já foi adicionado informações para este Tipo Benefício");

            var tipoBeneficioVM = Mapper.Map<TipoBeneficioViewModel>(_tipoBeneficioAplicacao.BuscarPorId(tipoBeneficioId));

            valor = _planoCarreiraAplicacao.BuscarValorPeloPeriodo(tipoBeneficioVM, funcionarioId);

            var beneficioFuncionario = new BeneficioFuncionarioDetalheViewModel(tipoBeneficioVM, valor);
            var listaBeneficioFuncionarioDetalhes = ListaBeneficioFuncionarioDetalhes;

            listaBeneficioFuncionarioDetalhes.Add(beneficioFuncionario);
            ListaBeneficioFuncionarioDetalhes = listaBeneficioFuncionarioDetalhes;

            var grid = RazorHelper.RenderRazorViewToString(ControllerContext, "_GridBeneficioFuncionarioDetalhes", ListaBeneficioFuncionarioDetalhes);
            return Json(new
            {
                Grid = grid,
            });
        }

        public ActionResult RemoverBeneficioFuncionario(int tipoBeneficioId, string guidSession)
        {
            GuidSession = guidSession;
            var item = ListaBeneficioFuncionarioDetalhes.FirstOrDefault(x => x.TipoBeneficio.Id == tipoBeneficioId);
            var listaBeneficioFuncionarioDetalhes = ListaBeneficioFuncionarioDetalhes;

            listaBeneficioFuncionarioDetalhes.Remove(item);
            ListaBeneficioFuncionarioDetalhes = listaBeneficioFuncionarioDetalhes;

            var grid = RazorHelper.RenderRazorViewToString(ControllerContext, "_GridBeneficioFuncionarioDetalhes", ListaBeneficioFuncionarioDetalhes);
            return Json(new
            {
                Grid = grid,
            });
        }

        public ActionResult EditarBeneficioFuncionario(int tipoBeneficioId, string guidSession)
        {
            GuidSession = guidSession;
            var item = ListaBeneficioFuncionarioDetalhes.FirstOrDefault(x => x.TipoBeneficio.Id == tipoBeneficioId);
            var listaBeneficioFuncionarioDetalhes = ListaBeneficioFuncionarioDetalhes;

            listaBeneficioFuncionarioDetalhes.Remove(item);
            ListaBeneficioFuncionarioDetalhes = listaBeneficioFuncionarioDetalhes;

            var grid = RazorHelper.RenderRazorViewToString(ControllerContext, "_GridBeneficioFuncionarioDetalhes", ListaBeneficioFuncionarioDetalhes);
            return Json(new
            {
                Grid = grid,
                Item = RemoverLoopDoJson(item)
            });
        }

        public ActionResult EditarSeJaExiste(int funcionarioId)
        {
            var beneficioFuncionario = _beneficioFuncionarioAplicacao.PrimeiroPor(x => x.Funcionario.Id == funcionarioId);

            if (beneficioFuncionario != null)
            {
                var beneficioFuncionarioVM = Mapper.Map<BeneficioFuncionarioViewModel>(beneficioFuncionario);
                ListaBeneficioFuncionarioDetalhes = beneficioFuncionarioVM.BeneficioFuncionarioDetalhes;
            }
            else
            {
                ListaBeneficioFuncionarioDetalhes = new List<BeneficioFuncionarioDetalheViewModel>();
            }

            var grid = RazorHelper.RenderRazorViewToString(ControllerContext, "_GridBeneficioFuncionarioDetalhes", ListaBeneficioFuncionarioDetalhes);

            return Json(new
            {
                Existe = beneficioFuncionario != null,
                beneficioFuncionario?.Id,
                Grid = grid
            }, JsonRequestBehavior.AllowGet);
        }
    }
}