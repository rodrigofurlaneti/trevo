using Entidade.Base;

namespace Entidade
{
    public class Mensalista : BaseEntity
    {
        public virtual string Descricao { get; set; }
        public virtual decimal Valor { get; set; }
    }
}
