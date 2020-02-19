using Entidade.Base;
using Entidade.Uteis;

namespace Entidade
{
    public class LimiteDesconto : BaseEntity
    {
        public virtual int ParametroNegociacao { get; set; }
        public virtual TipoServico TipoServico { get; set; }
        public virtual TipoValor TipoValor { get; set; }
        public virtual decimal Valor { get; set; }
    }
}
