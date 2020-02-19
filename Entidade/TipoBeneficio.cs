using System;
using System.IO;
using System.Linq;
using Entidade.Base;
using Entidade.Uteis;

namespace Entidade
{
    public class TipoBeneficio : BaseEntity
    {
        public virtual string Descricao { get; set; }
        public virtual bool Ativo { get; set; }
    }
}