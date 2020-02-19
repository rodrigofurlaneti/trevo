using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Entidade.Base;
using Entidade.Uteis;

namespace Entidade
{
    public class PedidoCompra : BaseEntity
    {
        public virtual FormaPagamentoPedidoCompra FormaPagamento { get; set; }
        public virtual TipoPagamentoPedidoCompra TipoPagamento { get; set; }
        public virtual Unidade Unidade { get; set; }
        public virtual Estoque Estoque { get; set; }
        public virtual StatusPedidoCompra Status { get; set; }
                        
        public virtual IList<PedidoCompraCotacaoMaterialFornecedor> PedidoCompraMaterialFornecedores { get; set; }
    }
}