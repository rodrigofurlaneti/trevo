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
    public class PedidoCompraController : GenericController<PedidoCompra>
    {
        private readonly IPedidoCompraAplicacao _pedidoCompraAplicacao;
        private readonly IUnidadeAplicacao _unidadeAplicacao;
        private readonly IEstoqueAplicacao _estoqueAplicacao;

        public List<CotacaoViewModel> ListaCotacao {
            get => (List<CotacaoViewModel>)Session["Cotacoes"];
            set => Session["Cotacoes"] = value;
        }

        public List<PedidoCompraCotacaoMaterialFornecedorViewModel> PedidoCompraCotacaoMaterialFornecedores
        {
            get => (List<PedidoCompraCotacaoMaterialFornecedorViewModel>)Session["PedidoCompraCotacaoMaterialFornecedor"];
            set => Session["PedidoCompraCotacaoMaterialFornecedor"] = value;
        }

        public IEnumerable<ChaveValorViewModel> ListaFormaPagamento => Aplicacao.BuscarValoresDoEnum<FormaPagamentoPedidoCompra>();
        public IEnumerable<ChaveValorViewModel> ListaTipoPagamento => Aplicacao.BuscarValoresDoEnum<TipoPagamentoPedidoCompra>();
        public List<UnidadeViewModel> ListaUnidade => _unidadeAplicacao.ListarOrdenadas();
        public List<EstoqueViewModel> ListaEstoque => Mapper.Map<List<EstoqueViewModel>>(_estoqueAplicacao.Buscar());

        public PedidoCompraController(
                IPedidoCompraAplicacao pedidoCompraAplicacao,
                IUnidadeAplicacao unidadeAplicacao,
                IEstoqueAplicacao estoqueAplicacao
            )
        {
            Aplicacao = pedidoCompraAplicacao;
            _pedidoCompraAplicacao = pedidoCompraAplicacao;
            _unidadeAplicacao = unidadeAplicacao;
            _estoqueAplicacao = estoqueAplicacao;
        }

        public override ActionResult Index()
        {
            PedidoCompraCotacaoMaterialFornecedores = new List<PedidoCompraCotacaoMaterialFornecedorViewModel>();
            ListaCotacao = _pedidoCompraAplicacao.BuscarCotacoes();
            return base.Index();
        }

        public override ActionResult Edit(int id)
        {
            ListaCotacao = _pedidoCompraAplicacao.BuscarCotacoes();
            var pedidoCompra = _pedidoCompraAplicacao.BuscarPorId(id);
            var pedidoCompraViewModel = Mapper.Map<PedidoCompraViewModel>(pedidoCompra);
            PedidoCompraCotacaoMaterialFornecedores = pedidoCompraViewModel.PedidoCompraMaterialFornecedores;
            return View("Index", pedidoCompraViewModel);
        }

        [HttpPost]
        public ActionResult SalvarDados(PedidoCompraViewModel pedidoCompraViewModel)
        {
            ListaCotacao = _pedidoCompraAplicacao.BuscarCotacoes();
            try
            {
                var usuarioLogadoCurrent = HttpContext.User as CustomPrincipal;
                pedidoCompraViewModel.PedidoCompraMaterialFornecedores = PedidoCompraCotacaoMaterialFornecedores;
                _pedidoCompraAplicacao.Salvar(pedidoCompraViewModel, usuarioLogadoCurrent.UsuarioId);
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

            return View("Index", pedidoCompraViewModel);
        }

        public ActionResult AtualizarCotacaoMaterialFornecedores(int cotacaoId)
        {
            var cotacaoMaterialFornecedores = ListaCotacao.FirstOrDefault(x => x.Id == cotacaoId)?.MaterialFornecedores;

            PedidoCompraCotacaoMaterialFornecedores.Clear();
            foreach (var item in cotacaoMaterialFornecedores)
            {
                PedidoCompraCotacaoMaterialFornecedores.Add(new PedidoCompraCotacaoMaterialFornecedorViewModel
                {
                    CotacaoMaterialFornecedor = item
                });
            }

            return PartialView("_GridPedidoCompraCotacaoMaterialFornecedores", PedidoCompraCotacaoMaterialFornecedores);
        }

        public ActionResult BuscarPedidoCompra()
        {
            var pedidoComprasViewModel = Mapper.Map<List<PedidoCompraViewModel>>(_pedidoCompraAplicacao.Buscar());
            return PartialView("_Grid", pedidoComprasViewModel);
        }

        public void AlterarStatusDoItem(int id)
        {
            var itemSelecionado = PedidoCompraCotacaoMaterialFornecedores.FirstOrDefault(x => x.CotacaoMaterialFornecedor.Id == id);
            itemSelecionado.Selecionado = !itemSelecionado.Selecionado;
        }
    }
}