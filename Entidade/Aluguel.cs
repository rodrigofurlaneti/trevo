using Entidade.Base;

namespace Entidade
{
    public class Aluguel : BaseEntity
    {
        public virtual string Descricao { get; set; }
        public virtual decimal Valor { get; set; }
    }
}
