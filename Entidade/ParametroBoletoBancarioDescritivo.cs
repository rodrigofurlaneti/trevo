using System;
using System.IO;
using System.Linq;
using Entidade.Base;
using Entidade.Uteis;

namespace Entidade
{
    public class ParametroBoletoBancarioDescritivo : BaseEntity, IAudit
    {
        public virtual string Descritivo { get; set; }
    }
}