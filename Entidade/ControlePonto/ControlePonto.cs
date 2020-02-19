using Entidade.Base;
using Entidade.Uteis;
using System;
using System.Collections.Generic;

namespace Entidade
{
    public class ControlePonto : BaseEntity
    {
        public virtual Funcionario Funcionario { get; set; }
        public virtual IList<ControlePontoDia> ControlePontoDias { get; set; }
    }
}