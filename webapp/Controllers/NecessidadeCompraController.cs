using Aplicacao;
using Aplicacao.ViewModels;
using Core.Exceptions;
using Core.Enums;
using Entidade;
using Entidade.Uteis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using AutoMapper;
using Portal.Helpers;

namespace Portal.Controllers
{
    public class NecessidadeCompraController : GenericController<NecessidadeCompra>
    {
        private readonly INecessidadeCompraAplicacao _necessidadeCompraAplicacao;
        private readonly IMaterialAplicacao _materialAplicacao;
        private readonly IFornecedorAplicacao _fornecedorAplicacao;

        public List<MaterialViewModel> ListaMaterial => Mapper.Map<List<MaterialViewModel>>(_materialAplicacao.Buscar().ToList());

        public List<NecessidadeCompraMaterialFornecedorViewModel> ListaNecessidadeCompraMaterialFornecedores
        {
            get => (List<NecessidadeCompraMaterialFornecedorViewModel>)Session["NecessidadeCompraMaterialFornecedores"];
            set => Session["NecessidadeCompraMaterialFornecedores"] = value;
        }

        public List<CotacaoMaterialFornecedorViewModel> ListaCotacaoMaterialFornecedores
        {
            get => (List<CotacaoMaterialFornecedorViewModel>)Session["CotacaoMaterialFornecedores"];
            set => Session["CotacaoMaterialFornecedores"] = value;
        }

        public NecessidadeCompraController(
                INecessidadeCompraAplicacao necessidadeCompraAplicacao,
                IMaterialAplicacao materialAplicacao,
                IFornecedorAplicacao fornecedorAplicacao
            )
        {
            Aplicacao = necessidadeCompraAplicacao;
            _necessidadeCompraAplicacao = necessidadeCompraAplicacao;
            _materialAplicacao = materialAplicacao;
            _fornecedorAplicacao = fornecedorAplicacao;
        }

        public override ActionResult Index()
        {
            ListaNecessidadeCompraMaterialFornecedores = null;
            ListaCotacaoMaterialFornecedores = null;
            return base.Index();
        }

        public override ActionResult Edit(int id)
        {
            var necessidadeCompra = _necessidadeCompraAplicacao.BuscarPorId(id);
            var necessidadeCompraViewModel = Mapper.Map<NecessidadeCompraViewModel>(necessidadeCompra);
            ListaNecessidadeCompraMaterialFornecedores = necessidadeCompraViewModel.MaterialFornecedores;

            return View("Index", necessidadeCompraViewModel);
        }

        [HttpPost]
        public ActionResult SalvarDados(NecessidadeCompraViewModel necessidadeCompraViewModel)
        {
            try
            {
                var usuarioLogadoCurrent = HttpContext.User as CustomPrincipal;

                necessidadeCompraViewModel.MaterialFornecedores = ListaNecessidadeCompraMaterialFornecedores;
                _necessidadeCompraAplicacao.Salvar(necessidadeCompraViewModel, usuarioLogadoCurrent.UsuarioId);
                CriarDadosModalSucesso("Registro salvo com sucesso!");
            }
            catch (BusinessRuleException br)
            {
                CriarDadosModalAviso(br.Message);
            }
            catch (Exception ex)
            {
                CriarDadosModalErro(ex.Message);
            }

            return View("Index", necessidadeCompraViewModel);
        }

        public ActionResult BuscarNecessidadeCompras()
        {
            var necessidadeCompras = Mapper.Map<List<NecessidadeCompraViewModel>>(_necessidadeCompraAplicacao.Buscar());
            return PartialView("_Grid", necessidadeCompras);
        }

        public ActionResult BuscarFornecedoresDoMaterial(int materialId)
        {
            if (materialId == 0)
                return PartialView("_Fornecedores", new List<FornecedorViewModel>());

            var material = _materialAplicacao.BuscarPorId(materialId);
            var fornecedores = material.MaterialFornecedores.Select(x => new FornecedorViewModel(x.Fornecedor)).ToList();

            return PartialView("_Fornecedores", fornecedores);
        }

        public JsonResult BuscarDadosDoModal(int necessidadeCompraId)
        {
            var necessidadeCompraViewModel = Mapper.Map<NecessidadeCompraViewModel>(_necessidadeCompraAplicacao.BuscarPorId(necessidadeCompraId));

            if (necessidadeCompraViewModel.Cotacao != null)
            {
                ListaCotacaoMaterialFornecedores = necessidadeCompraViewModel.Cotacao.MaterialFornecedores;
            }
            else
            {
                var lista = new List<CotacaoMaterialFornecedorViewModel>();
                var id = 0;
                foreach (var item in necessidadeCompraViewModel.MaterialFornecedores)
                {
                    lista.Add(new CotacaoMaterialFornecedorViewModel
                    {
                        Id = --id,
                        Fornecedor = item.Fornecedor,
                        Material = item.Material,
                        Quantidade = item.Quantidade
                    });
                }

                ListaCotacaoMaterialFornecedores = lista;
            }

            var grid = RazorHelper.RenderRazorViewToString(ControllerContext, "_GridCotacaoMaterialFornecedores", ListaCotacaoMaterialFornecedores);

            ListaCotacaoMaterialFornecedores.ForEach(x =>
            {
                if (x.Cotacao != null)
                    x.Cotacao.MaterialFornecedores = null;

                x.Material.MaterialFornecedores = null;
            });

            return Json(new
            {
                Grid = grid,
                MaterialFornecedores = ListaCotacaoMaterialFornecedores,
                PodeSolicitar = necessidadeCompraViewModel.Cotacao == null || necessidadeCompraViewModel.Cotacao.Id == 0 || necessidadeCompraViewModel.Cotacao.Status == StatusCotacao.Reprovado
            });
        }

        public ActionResult AtualizarMaterialFornecedores(List<NecessidadeCompraMaterialFornecedorViewModel> materialFornecedores)
        {
            ListaNecessidadeCompraMaterialFornecedores = materialFornecedores;

            return PartialView("_GridMaterialFornecedor", ListaNecessidadeCompraMaterialFornecedores);
        }

        public ActionResult AtualizarCotacaoMaterialFornecedores(List<CotacaoMaterialFornecedorViewModel> cotacaoMaterialFornecedores)
        {
            ListaCotacaoMaterialFornecedores = cotacaoMaterialFornecedores;

            return PartialView("_GridCotacaoMaterialFornecedores", ListaCotacaoMaterialFornecedores);
        }

        public ActionResult Cotacao(int id)
        {
            return View("Index");
        }

        public JsonResult BuscarNecessidadeCompraMaterialFornecedores()
        {
            ListaNecessidadeCompraMaterialFornecedores.ForEach(x =>
            {
                if (x.NecessidadeCompra.Cotacao != null)
                    x.NecessidadeCompra.Cotacao.MaterialFornecedores = null;

                x.NecessidadeCompra.MaterialFornecedores = null;
                x.Material.MaterialFornecedores = null;
            });
            return Json(ListaNecessidadeCompraMaterialFornecedores);
        }

        public ActionResult SalvarCotacao(int necessidadeCompraId)
        {
            try
            {
                var usuarioLogadoCurrent = HttpContext.User as CustomPrincipal;
                _necessidadeCompraAplicacao.SalvarCotacao(necessidadeCompraId, ListaCotacaoMaterialFornecedores, usuarioLogadoCurrent.UsuarioId);

                CriarDadosModalSucesso("Registro salvo com sucesso!");
            }
            catch (BusinessRuleException br)
            {
                CriarDadosModalAviso(br.Message);
            }
            catch (Exception ex)
            {
                CriarDadosModalAviso(ex.Message);
            }

            return PartialView("../Shared/_ModalPrincipal");
        }
    }
}