using Entidade.Base;
using Entidade.Uteis;
using System;
using System.Collections.Generic;

namespace Entidade
{
    public class CalendarioRH : BaseEntity
    {
        public virtual DateTime Data { get; set; }
        public virtual string Descricao { get; set; }
        public virtual Unidade Unidade { get; set; }
        public virtual bool DataFixa { get; set; }
        public virtual bool TodasUnidade { get; set; }
        public virtual IList<CalendarioRHUnidade> CalendarioRHUnidades { get; set; }
    }
}