using Entidade.Base;
using Entidade.Uteis;
using System;
using System.Collections.Generic;

namespace Entidade
{
    public class ControleFerias : BaseEntity
    {
        public virtual bool AutorizadoTrabalhar { get; set; }
        public virtual DateTime DataInicial { get; set; }
        public virtual DateTime DataFinal { get; set; }
        public virtual IList<ControleFeriasPeriodoPermitido> ListaPeriodoPermitido { get; set; }

        public virtual Funcionario Funcionario { get; set; }
    }
}