using Entidade.Base;

namespace Entidade
{
    public class PessoaContato : BaseEntity
    {
        public virtual Contato Contato { get; set; }
    }
}