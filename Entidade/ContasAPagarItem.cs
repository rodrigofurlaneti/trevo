using Entidade.Base;
using System;

namespace Entidade
{
    public class ContasAPagarItem : BaseEntity
    {
        public virtual ContaContabil ContaContabil { get; set; }
        public virtual Unidade Unidade { get; set; }
        public virtual decimal Valor { get; set; }
    }
}