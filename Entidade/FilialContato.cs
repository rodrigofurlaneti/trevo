using Entidade.Base;

namespace Entidade
{
    public class FilialContato : BaseEntity
    {
        public virtual Contato Contato { get; set; }
    }
}
