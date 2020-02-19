using Entidade.Base;

namespace Entidade
{
    public class ContratoMensalistaVeiculo : BaseEntity
    {
        public virtual ContratoMensalista ContratoMensalista { get; set; }

        public virtual Veiculo Veiculo { get; set; }
    }
}
