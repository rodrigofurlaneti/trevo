using Entidade.Base;

namespace Entidade
{
    public class TrabalhoEndereco : BaseEntity
    {
        public virtual Endereco Endereco { get; set; }
    }
}