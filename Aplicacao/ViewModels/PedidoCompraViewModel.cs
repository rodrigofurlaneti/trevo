using Entidade;
using Entidade.Uteis;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Aplicacao.ViewModels
{
    public class PedidoCompraViewModel
    {
        public int Id { get; set; }
        public DateTime DataInsercao { get; set; }

        public FormaPagamentoPedidoCompra FormaPagamento { get; set; }
        public TipoPagamentoPedidoCompra TipoPagamento { get; set; }
        public UnidadeViewModel Unidade { get; set; }
        public EstoqueViewModel Estoque { get; set; }
        public StatusPedidoCompra Status { get; set; }

        public int CotacaoId => PedidoCompraMaterialFornecedores?.FirstOrDefault()?.CotacaoMaterialFornecedor?.Cotacao?.Id ?? 0;

        public virtual List<PedidoCompraCotacaoMaterialFornecedorViewModel> PedidoCompraMaterialFornecedores { get; set; }

        public string Materiais => PedidoCompraMaterialFornecedores != null ? string.Join("; ", PedidoCompraMaterialFornecedores.Where(x => x.Selecionado).Select(x => x.CotacaoMaterialFornecedor.Material.Nome).Distinct()) : string.Empty;
        public string Fornecedores => PedidoCompraMaterialFornecedores != null ? string.Join("; ", PedidoCompraMaterialFornecedores.Where(x => x.Selecionado).Select(x => x.CotacaoMaterialFornecedor.Fornecedor.Nome).Distinct()) : string.Empty;
    }
}