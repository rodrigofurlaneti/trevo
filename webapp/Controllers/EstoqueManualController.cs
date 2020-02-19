using Aplicacao;
using Aplicacao.ViewModels;
using AutoMapper;
using Core.Exceptions;
using Core.Extensions;
using Entidade;
using Entidade.Uteis;
using Portal.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace Portal.Controllers
{
    public class EstoqueManualController : GenericController<EstoqueManual>
    {
        private readonly IEstoqueAplicacao _estoqueAplicacao;
        private readonly IMaterialAplicacao _materialAplicacao;
        private readonly IUnidadeAplicacao _unidadeAplicacao;
        private readonly IEstoqueManualAplicacao _estoqueManualAplicacao;
        private readonly IPedidoCompraAplicacao _pedidoCompraAplicacao;

        public List<PedidoCompraViewModel> ListaPedidoCompra => Mapper.Map<List<PedidoCompraViewModel>>(_pedidoCompraAplicacao.BuscarPor(x => x.Status == StatusPedidoCompra.AguardandoEntregaPedido));

        public List<EstoqueManualItemViewModel> EstoqueManualItensGerados
        {
            get => (List<EstoqueManualItemViewModel>)Session["EstoqueManualItensGerados"];
            set => Session["EstoqueManualItensGerados"] = value;
        }

        public List<EstoqueManualItemViewModel> EstoqueManualItensParaSaida
        {
            get => (List<EstoqueManualItemViewModel>)Session["EstoqueManualItensParaSaida"];
            set => Session["EstoqueManualItensParaSaida"] = value;
        }

        public List<EstoqueManualItemViewModel> EstoqueManualItensParaInventario
        {
            get => (List<EstoqueManualItemViewModel>)Session["EstoqueManualItensParaInventario"];
            set => Session["EstoqueManualItensParaInventario"] = value;
        }

        public List<UnidadeViewModel> ListaUnidade
        {
            get => (List<UnidadeViewModel>)Session["ListaUnidade"];
            set => Session["ListaUnidade"] = value;
        }

        public List<EstoqueViewModel> ListaEstoque
        {
            get => (List<EstoqueViewModel>)Session["ListaEstoque"];
            set => Session["ListaEstoque"] = value;
        }

        public List<EstoqueManual> ListaEstoqueManual => _estoqueManualAplicacao?.Buscar()?.ToList() ?? new List<EstoqueManual>();
        public List<MaterialViewModel> ListaMaterial => AutoMapper.Mapper.Map<List<Material>, List<MaterialViewModel>>(_materialAplicacao.Buscar().ToList());

        public EstoqueManualController(IEstoqueManualAplicacao estoqueManualAplicacao,
                                               IUnidadeAplicacao unidadeAplicacao,
                                               IEstoqueAplicacao estoqueAplicacao,
                                               IMaterialAplicacao materialAplicacao,
                                               IEstoqueManualItemAplicacao estoqueManualItemAplicacao,
                                               IPedidoCompraAplicacao pedidoCompraAplicacao)
        {
            Aplicacao = estoqueManualAplicacao;
            _estoqueManualAplicacao = estoqueManualAplicacao;
            _unidadeAplicacao = unidadeAplicacao;
            _estoqueAplicacao = estoqueAplicacao;
            _materialAplicacao = materialAplicacao;
            _pedidoCompraAplicacao = pedidoCompraAplicacao;
        }

        public override ActionResult Index()
        {
            EstoqueManualItensGerados = new List<EstoqueManualItemViewModel>();
            EstoqueManualItensParaSaida = new List<EstoqueManualItemViewModel>();
            EstoqueManualItensParaInventario = new List<EstoqueManualItemViewModel>();
            ListaUnidade = _unidadeAplicacao.ListarOrdenadas();
            ListaEstoque = Mapper.Map<List<EstoqueViewModel>>(_estoqueAplicacao.Buscar().ToList());
            return base.Index();
        }


        public ActionResult View(int id)
        {
            var estoqueManual = new EstoqueManualViewModel(Aplicacao.BuscarPorId(id));
            ListaUnidade = _unidadeAplicacao.ListarOrdenadas();
            ListaEstoque = Mapper.Map<List<EstoqueViewModel>>(_estoqueAplicacao.Buscar().ToList());

            return View("Index", estoqueManual);
        }

        [CheckSessionOut]
        [HttpPost]
        public ActionResult SalvarDados(EstoqueManualViewModel model)
        {
            try
            {
                ListaEstoque = Mapper.Map<List<EstoqueViewModel>>(_estoqueAplicacao.Buscar().ToList());
                var usuarioLogadoCurrent = HttpContext.User as CustomPrincipal;

                var material = _materialAplicacao.BuscarPorId(model.Material.Id);
                if (!EstoqueManualItensGerados.Any() && model.Acao == AcaoEstoqueManual.Entrada && material.EhUmAtivo)
                    throw new BusinessRuleException("Não é possível Salvar uma entrada sem gerar os itens!");

                if (!EstoqueManualItensParaSaida.Any() && model.Acao == AcaoEstoqueManual.Saida && material.EhUmAtivo)
                    throw new BusinessRuleException("Selecione pelo menos um item para Salvar!");

                model.ListEstoqueManualItem = EstoqueManualItensGerados;
                var estoqueManual = Mapper.Map<EstoqueManual>(model);
                _estoqueManualAplicacao.Salvar(estoqueManual, usuarioLogadoCurrent.UsuarioId, model.Material.Id);

                ModelState.Clear();
                CriarDadosModalSucesso("Registro salvo com sucesso!");
            }
            catch (BusinessRuleException br)
            {
                ModelState.Clear();
                CriarDadosModalAviso(br.Message);
            }
            catch (Exception ex)
            {
                ModelState.Clear();
                CriarDadosModalErro(ex.Message);
            }

            return View("Index", model);
        }

        public ActionResult GerarItens(int quantidade, int estoqueId, int materialId, int? unidadeId)
        {
            EstoqueManualItensGerados = _estoqueManualAplicacao.GerarItens(quantidade, estoqueId, materialId, unidadeId);

            return PartialView("_GridItem", EstoqueManualItensGerados);
        }

        public ActionResult AtualizarExibicaoEntrada()
        {
            EstoqueManualItensGerados = new List<EstoqueManualItemViewModel>();
            EstoqueManualItensParaSaida = new List<EstoqueManualItemViewModel>();
            EstoqueManualItensParaInventario = new List<EstoqueManualItemViewModel>();
            return PartialView("_GridItem", new List<EstoqueManualItemViewModel>());
        }

        public ActionResult AtualizarExibicaoSaida(EstoqueManualViewModel filtro)
        {
            EstoqueManualItensParaSaida = new List<EstoqueManualItemViewModel>();
            EstoqueManualItensParaInventario = new List<EstoqueManualItemViewModel>();
            var estoqueManualItensGeradosVM = new List<EstoqueManualItemViewModel>();
            try
            {
                var estoqueManual = _estoqueManualAplicacao
                    .BuscarPor(em => em.ListEstoqueManualItem
                        .Any(x => x.Estoque.Id == filtro.Estoque.Id && x.Material.Id == filtro.Material.Id && !x.Inventariado));


                estoqueManualItensGeradosVM = estoqueManual.SelectMany(x => x.ListEstoqueManualItem)
                                                           .Where(x => x.Estoque.Id == filtro.Estoque.Id && x.Material.Id == filtro.Material.Id && !x.Inventariado)
                                                           .Select(x => new EstoqueManualItemViewModel(x)).ToList();

                EstoqueManualItensGerados = estoqueManualItensGeradosVM;
            }
            catch (BusinessRuleException br)
            {
                ModelState.Clear();
                CriarDadosModalAviso(br.Message);
            }
            catch (Exception ex)
            {
                ModelState.Clear();
                CriarDadosModalErro(ex.Message);
            }

            return PartialView("_GridItem", estoqueManualItensGeradosVM);
        }

        public void ArmazenarItensParaSaida(int id, int estoqueId, int unidadeId)
        {
            var itemSelecionado = EstoqueManualItensGerados.FirstOrDefault(x => x.Id == id);

            if(EstoqueManualItensParaSaida.Any(x => x.Id == id))
            {
                itemSelecionado.Unidade = null;
                itemSelecionado.Estoque = ListaEstoque.FirstOrDefault(x => x.Id == estoqueId);
            }
            else
            {
                itemSelecionado.Estoque = null;
                itemSelecionado.Unidade = ListaUnidade.FirstOrDefault(x => x.Id == unidadeId);
            }

            EstoqueManualItensParaSaida = EstoqueManualItensGerados.Where(x => x.Unidade != null && x.Unidade.Id > 0).ToList();
        }

        public void ArmazenarItensParaInventario(int id)
        {
            var itemSelecionado = EstoqueManualItensGerados.FirstOrDefault(x => x.Id == id);
            itemSelecionado.Inventariado = !itemSelecionado.Inventariado;

            EstoqueManualItensParaInventario = EstoqueManualItensGerados.Where(x => x.Inventariado).ToList();
        }

        public JsonResult ChecarSeEhUmAtivo(int materialId)
        {
            return Json(ListaMaterial.FirstOrDefault(x => x.Id == materialId).EhUmAtivo);
        }

        public ActionResult BuscarPedidoCompraMaterialFornecedores(int pedidoCompraId)
        {
            var pedidoCompra = _pedidoCompraAplicacao.BuscarPorId(pedidoCompraId);
            var pedidoCompraMaterialFornecedores = pedidoCompra?.PedidoCompraMaterialFornecedores != null ? 
                Mapper.Map<List<PedidoCompraCotacaoMaterialFornecedorViewModel>>(pedidoCompra.PedidoCompraMaterialFornecedores.Where(x => !x.EntregaRealizada)) :
                new List<PedidoCompraCotacaoMaterialFornecedorViewModel>();
            var materiais = pedidoCompraMaterialFornecedores.Select(x => x.CotacaoMaterialFornecedor.Material).DistinctBy(x => x.Id).ToList();
            return PartialView("_Materiais", materiais.Count == 0 ? this.ListaMaterial : materiais);
        }

        public int BuscarQuantidade(int pedidoCompraId, int materialId)
        {
            var pedidoCompra = _pedidoCompraAplicacao.BuscarPorId(pedidoCompraId);
            return pedidoCompra.PedidoCompraMaterialFornecedores
                                        .Where(x => x.CotacaoMaterialFornecedor.Material.Id == materialId)
                                        .Sum(x => x.CotacaoMaterialFornecedor.Quantidade);
        }
    }
}