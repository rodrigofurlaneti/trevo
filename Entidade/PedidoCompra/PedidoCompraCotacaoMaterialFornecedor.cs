using System;
using System.ComponentModel.DataAnnotations;
using Entidade.Base;
using Entidade.Uteis;

namespace Entidade
{
    public class PedidoCompraCotacaoMaterialFornecedor : BaseEntity
    {
        public virtual PedidoCompra PedidoCompra { get; set; }
        public virtual CotacaoMaterialFornecedor CotacaoMaterialFornecedor { get; set; }

        public virtual bool Selecionado { get; set; }
        public virtual bool EntregaRealizada { get; set; }
    }
}