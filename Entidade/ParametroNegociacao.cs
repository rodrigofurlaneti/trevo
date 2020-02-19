using Entidade.Base;
using System.Collections.Generic;

namespace Entidade
{
    public class ParametroNegociacao : BaseEntity
    {
        public virtual Unidade Unidade { get; set; }
        public virtual Perfil Perfil { get; set; }
        //public virtual Usuario Usuario { get; set; }
        public virtual IList<LimiteDesconto> LimitesDesconto { get; set; }
    }
}
