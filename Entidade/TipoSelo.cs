using Entidade.Base;
using Entidade.Uteis;

namespace Entidade
{
    public class TipoSelo : BaseEntity
    {
        public virtual string Nome { get; set; }
        public virtual decimal Valor { get; set; }
        public virtual bool ComValidade { get; set; }
        public virtual bool PagarHorasAdicionais { get; set; }
        public virtual bool Ativo { get; set; }
        public virtual ParametroSelo ParametroSelo { get; set; }
    }
}