using System.Collections.Generic;
using Entidade.Base;
using Entidade.Uteis;

namespace Entidade
{
    public class ParametroBoletoBancario : BaseEntity, IAudit
    {
        public virtual TipoServico TipoServico { get; set; }
        public virtual int DiasAntesVencimento { get; set; }
        public virtual decimal ValorDesconto { get; set; }
        public virtual Unidade Unidade { get; set; }

        public virtual IList<ParametroBoletoBancarioDescritivo> ParametroBoletoBancarioDescritivos { get; set; }
    }
}