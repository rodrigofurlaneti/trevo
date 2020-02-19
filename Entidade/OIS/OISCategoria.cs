using Entidade.Base;
using Entidade.Uteis;

namespace Entidade
{
    public class OISCategoria
    {
        public virtual TipoOISCategoria TipoCategoria { get; set; }
        public virtual string OutraCategoria { get; set; }
    }
}