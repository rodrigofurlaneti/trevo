using Entidade.Base;
using Entidade.Uteis;
using System;
using System.Collections.Generic;

namespace Entidade
{
    public class ControleFeriasPeriodoPermitido
    {
        public virtual DateTime DataDe { get; set; }
        public virtual DateTime DataAte { get; set; }
    }
}