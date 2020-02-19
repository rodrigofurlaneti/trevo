using Entidade.Base;
using Entidade.Uteis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidade
{
    public class Remanejamento : BaseEntity
    {
        public virtual bool Fixo { get; set; }
        public virtual TipoOpreracao TipoOpreracao { get; set; }
        public virtual DateTime? DataFim { get; set; }
        public virtual RemanejamentoTransferencia RemanejamentoOrigem { get; set; }
        public virtual RemanejamentoTransferencia RemanejamentoDestino { get; set; }
    }
}
