using Entidade.Base;

namespace Entidade
{
    public class Proposta : BaseEntity
    {
        public virtual Cliente Cliente { get; set; }
        public virtual Unidade Unidade { get; set; }
        public virtual string Email { get; set; }
    }
}