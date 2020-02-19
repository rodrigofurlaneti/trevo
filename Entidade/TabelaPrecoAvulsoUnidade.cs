using Entidade.Base;

namespace Entidade
{
    public class TabelaPrecoAvulsoUnidade : BaseEntity
    {
        public virtual Unidade Unidade { get; set; }
        public virtual TabelaPrecoAvulso TabelaPrecoAvulso { get; set; }
        public virtual string HoraInicio { get; set; }
        public virtual string HoraFim { get; set; }
        public virtual decimal ValorDiaria { get; set; }
    }
}