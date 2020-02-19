
using Entidade.Base;
using Entidade.Uteis;

namespace Entidade
{
    public class SeloCliente : BaseEntity
    {
        public virtual Cliente Cliente { get; set; }
        public virtual TipoPagamentoSelo TipoPagamentoSelo { get; set; }
        public virtual int ValidadeSelo { get; set; }
        public virtual int PrazoPagamentoSelo { get; set; }
        public virtual bool EmissaoNF { get; set; }
    }
}
