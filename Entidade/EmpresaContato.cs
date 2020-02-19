using Entidade.Base;

namespace Entidade
{
    public class EmpresaContato : BaseEntity
    {
        public virtual Contato Contato { get; set; }
    }
}