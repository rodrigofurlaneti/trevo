using Aplicacao;
using Aplicacao.ViewModels;
using AutoMapper;
using Core.Exceptions;
using Entidade;
using Entidade.Uteis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;

namespace Portal.Controllers
{
    public class AberturaOISController : GenericController<OIS>
    {
        private readonly IOISAplicacao _oisAplicacao;
        private readonly IMarcaAplicacao _marcaAplicacao;
        private readonly IModeloAplicacao _modeloAplicacao;
        private readonly IUnidadeAplicacao _unidadeAplicacao;
        private readonly IFuncionarioAplicacao _funcionarioAplicacao;

        public List<Marca> ListaMarca => _marcaAplicacao.Buscar().ToList();
        public List<Modelo> ListaModelo => _modeloAplicacao.Buscar().ToList();
        public List<ChaveValorViewModel> ListaTipoVeiculo => _oisAplicacao.BuscarValoresDoEnum<TipoVeiculo>().ToList();
        public List<ChaveValorViewModel> ListaStatusSinistro => _oisAplicacao.BuscarValoresDoEnum<StatusSinistro>().ToList();
        public List<Unidade> ListaUnidade => _unidadeAplicacao.Buscar().ToList();
        public List<Funcionario> ListaFuncionario => _funcionarioAplicacao.Buscar().ToList();
        public List<ChaveValorViewModel> ListaCategoria => _oisAplicacao.BuscarValoresDoEnum<TipoOISCategoria>().ToList();

        public List<OISFuncionarioViewModel> ListaOisFuncionario
        {
            get => (List<OISFuncionarioViewModel>)Session["OISFuncionarios"];
            set => Session["OISFuncionarios"] = value;
        }

        public List<OISCategoriaViewModel> ListaOisCategoria
        {
            get => (List<OISCategoriaViewModel>)Session["OISCategorias"];
            set => Session["OISCategorias"] = value;
        }

        public AberturaOISController(IOISAplicacao oisAplicacao
                                , IMarcaAplicacao marcaAplicacao
                                , IModeloAplicacao modeloAplicacao
                                , IUnidadeAplicacao unidadeAplicacao
                                , IFuncionarioAplicacao funcionarioAplicacao
                                )
        {
            Aplicacao = oisAplicacao;
            _oisAplicacao = oisAplicacao;
            _marcaAplicacao = marcaAplicacao;
            _modeloAplicacao = modeloAplicacao;
            _unidadeAplicacao = unidadeAplicacao;
            _funcionarioAplicacao = funcionarioAplicacao;
        }

        public override ActionResult Index()
        {
            ListaOisFuncionario = null;
            ListaOisCategoria = null;
            return base.Index();
        }

        public override ActionResult Edit(int id)
        {
            var ois = _oisAplicacao.BuscarPorId(id);
            var oisViewModel = Mapper.Map<OISViewModel>(ois);
            ListaOisFuncionario = oisViewModel.OISFuncionarios;
            ListaOisCategoria = oisViewModel.OISCategorias;

            for (int i = 0; i < oisViewModel.OISImagens.Count; i++)
            {
                oisViewModel.ImagensParaSalvar[i].ImagemUpload = oisViewModel.OISImagens[i].ImagemUpload;
            }

            return View("Index", oisViewModel);
        }

        [HttpPost]
        public ActionResult SalvarDados(OISViewModel oisViewModel, List<OISImagemViewModel> imagens)
        {
            try
            {
                imagens = imagens.Where(x => !string.IsNullOrEmpty(x.ImagemUpload)).ToList();
                oisViewModel.OISImagens = imagens;
                oisViewModel.OISCategorias = ListaOisCategoria;
                oisViewModel.OISFuncionarios = ListaOisFuncionario;

                var ois = Mapper.Map<OIS>(oisViewModel);

                _oisAplicacao.Salvar(ois, UsuarioLogado.UsuarioId);

                ModelState.Clear();
                CriarDadosModalSucesso("Registro salvo com sucesso");
            }
            catch (BusinessRuleException br)
            {
                CriarDadosModalAviso(br.Message);
            }
            catch (Exception ex)
            {
                CriarDadosModalErro(ex.Message);
            }

            return View("Index", oisViewModel);
        }

        public JsonResult BuscarOisFuncionarios()
        {
            return Json(ListaOisFuncionario);
        }

        public ActionResult AtualizarOisFuncionarios(List<OISFuncionarioViewModel> oisFuncionarios)
        {
            ListaOisFuncionario = oisFuncionarios;

            return PartialView("_GridOISFuncionario", ListaOisFuncionario);
        }

        public JsonResult BuscarOisCategorias()
        {
            return Json(ListaOisCategoria);
        }

        public ActionResult AtualizarOisCategorias(List<OISCategoriaViewModel> oisCategorias)
        {
            ListaOisCategoria = oisCategorias;

            return PartialView("_GridOISCategoria", ListaOisCategoria);
        }

        public ActionResult BuscarOis()
        {
            var ois = _oisAplicacao.Buscar().ToList();
            var oisViewModel = Mapper.Map<List<OISViewModel>>(ois);
            return PartialView("_Grid", oisViewModel);
        }
    }
}