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
    public class OcorrenciaFuncionarioController : GenericController<OcorrenciaFuncionario>
    {
        private readonly IOcorrenciaFuncionarioAplicacao _ocorrenciaFuncionarioAplicacao;
        private readonly ITipoOcorrenciaAplicacao _tipoOcorrenciaAplicacao;
        private readonly IUnidadeAplicacao _unidadeAplicacao;
        private readonly IUsuarioAplicacao _usuarioAplicacao;

        public List<OcorrenciaFuncionarioDetalheViewModel> ListaOcorrenciaFuncionarioDetalhe
        {
            get => (List<OcorrenciaFuncionarioDetalheViewModel>)Session["ListaOcorrenciaFuncionarioDetalhe"] ?? new List<OcorrenciaFuncionarioDetalheViewModel>();
            set => Session["ListaOcorrenciaFuncionarioDetalhe"] = value;
        }

        public OcorrenciaFuncionarioController(
            IOcorrenciaFuncionarioAplicacao ocorrenciaFuncionarioAplicacao
            , ITipoOcorrenciaAplicacao tipoOcorrenciaAplicacao
            , IUnidadeAplicacao unidadeAplicacao
            , IUsuarioAplicacao usuarioAplicacao
        )
        {
            Aplicacao = ocorrenciaFuncionarioAplicacao;
            _ocorrenciaFuncionarioAplicacao = ocorrenciaFuncionarioAplicacao;
            _tipoOcorrenciaAplicacao = tipoOcorrenciaAplicacao;
            _unidadeAplicacao = unidadeAplicacao;
            _usuarioAplicacao = usuarioAplicacao;

            ViewBag.ListaTipoOcorrencia = _tipoOcorrenciaAplicacao.Buscar().ToList();
        }

        [CheckSessionOut]
        public override ActionResult Index()
        {
            ListaOcorrenciaFuncionarioDetalhe = new List<OcorrenciaFuncionarioDetalheViewModel>();
            return View("Index");
        }

        [CheckSessionOut]
        [HttpPost]
        public ActionResult SalvarDados(OcorrenciaFuncionarioViewModel model)
        {
            try
            {
                model.OcorrenciaFuncionarioDetalhes = ListaOcorrenciaFuncionarioDetalhe;
                var ocorrenciaFuncionario = Mapper.Map<OcorrenciaFuncionario>(model);
                ocorrenciaFuncionario.UsuarioResponsavel = _usuarioAplicacao.BuscarPorId(UsuarioLogado.UsuarioId);
                _ocorrenciaFuncionarioAplicacao.Salvar(ocorrenciaFuncionario);
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
            var ocorrenciaFuncionario = _ocorrenciaFuncionarioAplicacao.BuscarPorId(id);
            var ocorrenciaFuncionarioVM = Mapper.Map<OcorrenciaFuncionarioViewModel>(ocorrenciaFuncionario);
            ListaOcorrenciaFuncionarioDetalhe = ocorrenciaFuncionarioVM.OcorrenciaFuncionarioDetalhes;
            return View("Index", ocorrenciaFuncionarioVM);
        }

        public ActionResult AdicionarOcorrenciaFuncionarioDetalhe(
            int id,
            DateTime dataOcorrencia,
            string justificativa,
            int tipoOcorrenciaId,
            int unidadeId)
        {
            var tipoOcorrenciaVM = Mapper.Map<TipoOcorrenciaViewModel>(_tipoOcorrenciaAplicacao.BuscarPorId(tipoOcorrenciaId));
            var unidadeVM = Mapper.Map<UnidadeViewModel>(_unidadeAplicacao.BuscarPorId(unidadeId));
            var usuarioVM = Mapper.Map<UsuarioViewModel>(_usuarioAplicacao.BuscarPorId(UsuarioLogado.UsuarioId));
            var item = new OcorrenciaFuncionarioDetalheViewModel(id, dataOcorrencia, justificativa, tipoOcorrenciaVM, unidadeVM, usuarioVM);
            var listaOcorrenciaFuncionarioDetalhe = ListaOcorrenciaFuncionarioDetalhe;

            listaOcorrenciaFuncionarioDetalhe.Add(item);
            ListaOcorrenciaFuncionarioDetalhe = listaOcorrenciaFuncionarioDetalhe;

            var grid = RazorHelper.RenderRazorViewToString(ControllerContext, "_GridOcorrenciaFuncionarioDetalhes", ListaOcorrenciaFuncionarioDetalhe);
            return Json(new
            {
                Grid = grid,
                ValorTotal = ListaOcorrenciaFuncionarioDetalhe.Sum(x => decimal.Parse(x.TipoOcorrencia.Percentual)).ToString("N2")
            });
        }

        public ActionResult RemoverOcorrenciaFuncionarioDetalhe(int id)
        {
            var item = ListaOcorrenciaFuncionarioDetalhe.FirstOrDefault(x => x.Id == id);
            var listaOcorrenciaFuncionarioDetalhe = ListaOcorrenciaFuncionarioDetalhe;

            listaOcorrenciaFuncionarioDetalhe.Remove(item);
            ListaOcorrenciaFuncionarioDetalhe = listaOcorrenciaFuncionarioDetalhe;

            var grid = RazorHelper.RenderRazorViewToString(ControllerContext, "_GridOcorrenciaFuncionarioDetalhes", ListaOcorrenciaFuncionarioDetalhe);
            return Json(new
            {
                Grid = grid,
                ValorTotal = ListaOcorrenciaFuncionarioDetalhe.Sum(x => decimal.Parse(x.TipoOcorrencia.Percentual)).ToString("N2")
            });
        }

        public ActionResult EditarOcorrenciaFuncionarioDetalhe(int id)
        {
            var item = ListaOcorrenciaFuncionarioDetalhe.FirstOrDefault(x => x.Id == id);
            var listaOcorrenciaFuncionarioDetalhe = ListaOcorrenciaFuncionarioDetalhe;

            listaOcorrenciaFuncionarioDetalhe.Remove(item);
            ListaOcorrenciaFuncionarioDetalhe = listaOcorrenciaFuncionarioDetalhe;

            var grid = RazorHelper.RenderRazorViewToString(ControllerContext, "_GridOcorrenciaFuncionarioDetalhes", ListaOcorrenciaFuncionarioDetalhe);
            return Json(new
            {
                Grid = grid,
                Item = RemoverLoopDoJson(item),
                Data = item.DataOcorrencia.ToShortDateString(),
                ValorTotal = ListaOcorrenciaFuncionarioDetalhe.Sum(x => decimal.Parse(x.TipoOcorrencia.Percentual)).ToString("N2")
            });
        }

        public PartialViewResult BuscarOcorrenciaFuncionario()
        {
            var itens = _ocorrenciaFuncionarioAplicacao.BuscarPor(x => x.OcorrenciaFuncionarioDetalhes.Any());
            var itensVM = Mapper.Map<List<OcorrenciaFuncionarioViewModel>>(itens);

            return PartialView("_Grid", itensVM);
        }

        public JsonResult BuscarUsuarioLogado()
        {
            return Json(new
            {
                UsuarioLogado.Nome
            }, JsonRequestBehavior.AllowGet);
        }

        public ActionResult EditarSeJaExiste(int funcionarioId)
        {
            var ocorrenciaFuncionario = _ocorrenciaFuncionarioAplicacao.PrimeiroPor(x => x.Funcionario.Id == funcionarioId);

            if (ocorrenciaFuncionario != null)
            {
                var ocorrenciaFuncionarioVM = Mapper.Map<OcorrenciaFuncionarioViewModel>(ocorrenciaFuncionario);
                ListaOcorrenciaFuncionarioDetalhe = ocorrenciaFuncionarioVM.OcorrenciaFuncionarioDetalhes;
            }
            else
            {
                ListaOcorrenciaFuncionarioDetalhe = new List<OcorrenciaFuncionarioDetalheViewModel>();
            }

            var grid = RazorHelper.RenderRazorViewToString(ControllerContext, "_GridOcorrenciaFuncionarioDetalhes", ListaOcorrenciaFuncionarioDetalhe);

            return Json(new
            {
                Existe = ocorrenciaFuncionario != null,
                ocorrenciaFuncionario?.Id,
                Grid = grid,
                ValorTotal = ListaOcorrenciaFuncionarioDetalhe.Sum(x => decimal.Parse(x.TipoOcorrencia.Percentual)).ToString("N2")
            }, JsonRequestBehavior.AllowGet);
        }
    }
}