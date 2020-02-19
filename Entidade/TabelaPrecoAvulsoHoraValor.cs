using Entidade.Base;

namespace Entidade
{
    public class TabelaPrecoAvulsoHoraValor : BaseEntity
    {
        public virtual string Hora { get; set; }
        public virtual decimal Valor { get; set; }
        public virtual TabelaPrecoAvulso TabelaPrecoAvulso { get; set; }
    }
}