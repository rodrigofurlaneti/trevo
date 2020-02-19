using Entidade.Base;

namespace Entidade
{
    public class BeneficioFuncionarioDetalhe : BaseEntity
    {
        public virtual TipoBeneficio TipoBeneficio { get; set; }
        public virtual decimal Valor { get; set; }
    }
}