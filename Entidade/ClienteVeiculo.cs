using Entidade.Base;

namespace Entidade
{
    public class ClienteVeiculo : BaseEntity
    {
        public virtual Veiculo Veiculo { get; set; }
        public int Cliente { get; set; }
    }
}
