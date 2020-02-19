using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Entidade;
using Entidade.Base;
using Entidade.Uteis;

namespace Entidade
{
    public class PagamentoPedidoSelo : BaseEntity
    {
        public virtual Cliente Cliente { get; set; }
        public virtual Convenio Convenio { get; set; }
        public virtual Unidade Unidade { get; set; }
        public virtual TipoPagamentoSelo TiposPagamento { get; set; }
        public virtual TipoSelo TipoSelo { get; set; }
    }
}