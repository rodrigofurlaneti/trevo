using Entidade.Base;

namespace Entidade
{
    public class CondominoVeiculo : BaseEntity
    {
        public virtual Veiculo Veiculo { get; set; }
        public ClienteCondomino Condomino { get; set; }
    }
}
