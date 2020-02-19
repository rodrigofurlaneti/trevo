using Entidade.Base;

namespace Entidade
{
    public class FornecedorContato : BaseEntity
    {
        public virtual Contato Contato { get; set; }
    }
}
