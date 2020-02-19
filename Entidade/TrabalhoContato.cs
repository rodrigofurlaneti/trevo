using Entidade.Base;

namespace Entidade
{
    public class TrabalhoContato : BaseEntity
    {
        public virtual Contato Contato { get; set; }
    }
}