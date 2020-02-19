using Entidade.Base;
using Entidade.Uteis;

namespace Entidade
{
    public class PessoaDocumento : BaseEntity
    {
        public virtual Documento Documento { get; set; }
        public TipoDocumento Tipo { get; set; }
    }
}