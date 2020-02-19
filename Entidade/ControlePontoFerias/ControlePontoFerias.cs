using Entidade.Base;
using Entidade.Uteis;
using System;
using System.Collections.Generic;

namespace Entidade
{
    public class ControlePontoFerias : BaseEntity
    {
        public virtual Funcionario Funcionario { get; set; }
        public virtual IList<ControlePontoFeriasDia> ControlePontoFeriasDias { get; set; }
    }
}