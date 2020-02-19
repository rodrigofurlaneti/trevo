using Entidade.Base;

namespace Entidade
{
    public class TipoUnidade : BaseEntity
    {
        public virtual string Codigo { get; set; }
        public virtual string Descricao { get; set; }
    }
}
