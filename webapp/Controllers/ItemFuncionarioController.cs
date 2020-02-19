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
    public class ItemFuncionarioController : GenericController<ItemFuncionario>
    {
        private readonly IItemFuncionarioAplicacao _itemFuncionarioAplicacao;
        private readonly IFuncionarioAplicacao _funcionarioAplicacao;
        private readonly IMaterialAplicacao _materialAplicacao;

        public ItemFuncionarioImpressaoViewModel ItemImpressao
        {
            get => (ItemFuncionarioImpressaoViewModel)Session["ItemImpressao"] ?? new ItemFuncionarioImpressaoViewModel();
            set => Session["ItemImpressao"] = value;
        }

        public List<ItemFuncionarioDetalheViewModel> ListaFuncionarioItens
        {
            get => (List<ItemFuncionarioDetalheViewModel>)Session["ListaFuncionarioItens"] ?? new List<ItemFuncionarioDetalheViewModel>();
            set => Session["ListaFuncionarioItens"] = value;
        }

        public List<ItemFuncionarioDetalheViewModel> ItensSelecionados
        {
            get => (List<ItemFuncionarioDetalheViewModel>)Session["ItensSelecionados"] ?? new List<ItemFuncionarioDetalheViewModel>();
            set => Session["ItensSelecionados"] = value;
        }

        public ItemFuncionarioController(
            IItemFuncionarioAplicacao itemFuncionarioAplicacao
            , IFuncionarioAplicacao funcionarioAplicacao
            , IMaterialAplicacao materialAplicacao
        )
        {
            Aplicacao = itemFuncionarioAplicacao;
            _itemFuncionarioAplicacao = itemFuncionarioAplicacao;
            _funcionarioAplicacao = funcionarioAplicacao;
            _materialAplicacao = materialAplicacao;

            ViewBag.ListaMaterial = _materialAplicacao.Buscar().ToList();
        }

        [CheckSessionOut]
        public override ActionResult Index()
        {
            ListaFuncionarioItens = new List<ItemFuncionarioDetalheViewModel>();
            ItensSelecionados = new List<ItemFuncionarioDetalheViewModel>();
            return View("Index");
        }

        [CheckSessionOut]
        [HttpPost]
        public ActionResult SalvarDados(ItemFuncionarioViewModel model)
        {
            try
            {
                ItensSelecionados = new List<ItemFuncionarioDetalheViewModel>();
                model.ItemFuncionariosDetalhes = ListaFuncionarioItens;
                var itemFuncionario = Mapper.Map<ItemFuncionario>(model);
                _itemFuncionarioAplicacao.Salvar(itemFuncionario);
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
            ItensSelecionados = new List<ItemFuncionarioDetalheViewModel>();
            var itemFuncionario = _itemFuncionarioAplicacao.BuscarPorId(id);
            var itemFuncionarioVM = Mapper.Map<ItemFuncionarioViewModel>(itemFuncionario);
            ListaFuncionarioItens = itemFuncionarioVM.ItemFuncionariosDetalhes;
            return View("Index", itemFuncionarioVM);
        }

        public PartialViewResult BuscarEstoqueDoMaterial(int materialId)
        {
            var estoqueMaterial = _materialAplicacao.BuscarEstoqueMateriaisPeloMaterialId(materialId);
            var estoques = estoqueMaterial != null && estoqueMaterial.Count > 0 ? estoqueMaterial.Select(x => x.Estoque).ToList() : new List<EstoqueViewModel>();

            return PartialView("_Estoques", estoques);
        }

        public ActionResult AdicionarFuncionarioItem(int materialId, int estoqueId, string valor, int quantidade, string valorTotal)
        {
            var listaEstoqueMaterial = _materialAplicacao.BuscarEstoqueMateriaisPeloMaterialId(materialId);
            var estoqueMaterial = listaEstoqueMaterial.FirstOrDefault(x => x.Estoque.Id == estoqueId);

            if (ListaFuncionarioItens.Any(x => x.Material.Id == materialId && x.EstoqueMaterial.Estoque.Id == estoqueId))
                return new HttpStatusCodeResult(HttpStatusCode.PreconditionFailed, "Já foi adicionado informações para este item nesse estoque");

            var materialVM = Mapper.Map<MaterialViewModel>(_materialAplicacao.BuscarPorId(materialId));
            var item = new ItemFuncionarioDetalheViewModel(materialVM, estoqueMaterial, valor, quantidade, valorTotal);
            var listaFuncionarioItens = ListaFuncionarioItens;

            listaFuncionarioItens.Add(item);
            ListaFuncionarioItens = listaFuncionarioItens;

            var grid = RazorHelper.RenderRazorViewToString(ControllerContext, "_GridItensDetalhes", ListaFuncionarioItens);
            return Json(new
            {
                Grid = grid,
            });
        }

        public ActionResult RemoverFuncionarioItem(int estoqueMaterialId)
        {
            var item = ListaFuncionarioItens.FirstOrDefault(x => x.EstoqueMaterial.Id == estoqueMaterialId);
            var listaFuncionarioItens = ListaFuncionarioItens;

            listaFuncionarioItens.Remove(item);
            ListaFuncionarioItens = listaFuncionarioItens;

            var grid = RazorHelper.RenderRazorViewToString(ControllerContext, "_GridItensDetalhes", ListaFuncionarioItens);
            return Json(new
            {
                Grid = grid,
            });
        }

        public ActionResult EditarFuncionarioItem(int estoqueMaterialId)
        {
            var item = ListaFuncionarioItens.FirstOrDefault(x => x.EstoqueMaterial.Id == estoqueMaterialId);
            var listaFuncionarioItens = ListaFuncionarioItens;

            listaFuncionarioItens.Remove(item);
            ListaFuncionarioItens = listaFuncionarioItens;

            var grid = RazorHelper.RenderRazorViewToString(ControllerContext, "_GridItensDetalhes", ListaFuncionarioItens);
            return Json(new
            {
                Grid = grid,
                Item = RemoverLoopDoJson(item)
            });
        }

        public void AlternarItemSelecionado(int materialId)
        {
            var itensSelecionado = ItensSelecionados;

            if (itensSelecionado.Any(x => x.Material.Id == materialId))
            {
                ItensSelecionados.RemoveAll(x => x.Material.Id == materialId);
            }
            else
            {
                var itemSelecionado = ListaFuncionarioItens.FirstOrDefault(x => x.Material.Id == materialId);
                itensSelecionado.Add(itemSelecionado);
            }
            ItensSelecionados = ItensSelecionados;
        }

        public void AltenarSelecionarTudo(bool selecionado)
        {
            if (selecionado)
                ItensSelecionados = ListaFuncionarioItens;
            else
                ItensSelecionados = new List<ItemFuncionarioDetalheViewModel>();
        }

        public PartialViewResult BuscarItemFuncionario()
        {
            var itens = _itemFuncionarioAplicacao.BuscarPor(x => x.ItemFuncionariosDetalhes.Any());
            var itensVM = Mapper.Map<List<ItemFuncionarioViewModel>>(itens);

            return PartialView("_Grid", itensVM);
        }

        public HttpStatusCodeResult ArmazenarDadosImpressao(int? funcionarioId, DateTime? dataEntrega, DateTime? dataDevolucao)
        {
            if (funcionarioId == null)
                return new HttpStatusCodeResult(HttpStatusCode.PreconditionFailed, "Informe o Funcionário");

            if (!dataEntrega.HasValue)
                return new HttpStatusCodeResult(HttpStatusCode.PreconditionFailed, "Informe a Data de Entrega");

            if (!ItensSelecionados.Any())
                return new HttpStatusCodeResult(HttpStatusCode.PreconditionFailed, "Nenhum item Selecionado");

            var funcionarioVM = Mapper.Map<FuncionarioViewModel>(_funcionarioAplicacao.BuscarPorId(funcionarioId.Value));
            ItemImpressao = new ItemFuncionarioImpressaoViewModel(funcionarioVM, dataEntrega, dataDevolucao, ItensSelecionados);

            return new HttpStatusCodeResult(HttpStatusCode.OK);
        }

        public ActionResult Impressao()
        {
            return PartialView("Impressao", ItemImpressao);
        }

        public ActionResult EditarSeJaExiste(int funcionarioId)
        {
            var itemFuncionario = _itemFuncionarioAplicacao.PrimeiroPor(x => x.Funcionario.Id == funcionarioId);

            if (itemFuncionario != null)
            {
                var itemFuncionarioVM = Mapper.Map<ItemFuncionarioViewModel>(itemFuncionario);
                ListaFuncionarioItens = itemFuncionarioVM.ItemFuncionariosDetalhes;
            }
            else
            {
                ListaFuncionarioItens = new List<ItemFuncionarioDetalheViewModel>();
            }

            var grid = RazorHelper.RenderRazorViewToString(ControllerContext, "_GridItensDetalhes", ListaFuncionarioItens);

            return Json(new
            {
                Existe = itemFuncionario != null,
                itemFuncionario?.Id,
                Grid = grid,
                DataEntrega = itemFuncionario?.DataEntrega?.ToShortDateString(),
                ResponsavelEntregaNome = itemFuncionario?.ResponsavelEntrega?.PessoaNome,
                ResponsavelEntregaId = itemFuncionario?.ResponsavelEntrega?.Id,
                DataDevolucao = itemFuncionario?.DataDevolucao?.ToShortDateString(),
                ResponsavelDevolucaoNome = itemFuncionario?.ResponsavelDevolucao?.PessoaNome,
                ResponsavelDevolucaoId = itemFuncionario?.ResponsavelDevolucao?.Id
            }, JsonRequestBehavior.AllowGet);
        }
    }
}