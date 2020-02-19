using Entidade.Base;

namespace Entidade
{
    public class PessoaLoja : BaseEntity
    {
        public virtual Empresa Loja { get; set; }
        public virtual Pessoa Pessoa { get; set; }
    }
}