using Entidade;
using System;

namespace Aplicacao.ViewModels
{
    public class PedidoCompraCotacaoMaterialFornecedorViewModel
    {
        public int Id { get; set; }
        public DateTime DataInsercao { get; set; }
        public PedidoCompraViewModel PedidoCompra { get; set; }
        public CotacaoMaterialFornecedorViewModel CotacaoMaterialFornecedor { get; set; }

        public bool Selecionado { get; set; }
    }
}