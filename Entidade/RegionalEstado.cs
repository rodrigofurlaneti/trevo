using Entidade.Base;

namespace Entidade
{
    public class RegionalEstado : BaseEntity
    {
        public virtual Regional Regional { get; set; }
        public virtual Estado Estado { get; set; }
    }
}
