using System;
using System.IO;
using System.Linq;
using Entidade.Base;
using Entidade.Uteis;

namespace Entidade
{
    public class Banco : BaseEntity
    {
        public virtual string CodigoDescricao { get { return CodigoBanco + " - " + Descricao; } }

        public virtual string CodigoBanco { get; set; }

        public virtual string Descricao { get; set; }
    }
}