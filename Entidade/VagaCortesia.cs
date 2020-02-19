using Entidade.Base;
using Entidade.Uteis;
using System.Collections.Generic;

namespace Entidade
{
    public class VagaCortesia : BaseEntity
    {
        public virtual Cliente Cliente { get; set; }
        public virtual IList<VagaCortesiaVigencia> VagaCortesiaVigencia { get; set; }
    }
}