using Entidade.Base;
using System;

namespace Entidade
{
    public class BloqueioReferencia : BaseEntity, IAudit
    {
        public virtual DateTime DataMesAnoReferencia { get; set; }
        public virtual bool Ativo { get; set; }
    }
}
