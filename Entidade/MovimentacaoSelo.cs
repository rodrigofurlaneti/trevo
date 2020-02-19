using Entidade.Base;
using System;
using System.Collections.Generic;

namespace Entidade
{
    public class MovimentacaoSelo : BaseEntity
    {
        public virtual int? IdSoftpark { get; set; }
        public virtual int? MovimentacaoIdSoftpark { get; set; }
        public virtual Selo Selo { get; set; }
        public virtual Movimentacao Movimentacao { get; set; }
    }
}
