using Entidade.Base;
using Entidade.Uteis;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Entidade
{
    public class EstoqueManual : BaseEntity
    {
        public virtual string NumeroNFPedido { get; set; }
        public virtual int Quantidade { get; set; }
        public virtual decimal Preco { get; set; }
        public virtual decimal ValorTotal { get; set; }
        public virtual string Motivo { get; set; }

        public virtual AcaoEstoqueManual Acao { get; set; }

        public virtual PedidoCompra PedidoCompra { get; set; }
        public virtual Estoque Estoque { get; set; }
        public virtual Material Material { get; set; }
        public virtual Unidade Unidade { get; set; }

        public virtual IList<EstoqueManualItem> ListEstoqueManualItem { get; set; }
    }
}