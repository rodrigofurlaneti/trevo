using Entidade.Base;
using Entidade.Uteis;
using System;
using System.Collections.Generic;

namespace Entidade
{
    public class PlanoCarreira : BaseEntity
    {
        public virtual string Descricao { get; set; }
        public virtual decimal Valor { get; set; }
        public virtual decimal AnoDe { get; set; }
        public virtual decimal AnoAte { get; set; }
    }
}