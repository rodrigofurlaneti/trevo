using System.Collections.Generic;
using Entidade.Base;

namespace Entidade
{
    public class DevedorArquivoImportacao : BaseEntity
    {
        public virtual Devedor Devedor { get; set; }
        public virtual ArquivoImportacao ArquivoImportacao { get; set; }
    }
}