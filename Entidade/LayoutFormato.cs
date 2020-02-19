using System.Collections.Generic;
using System.Linq;
using Entidade.Base;
using Entidade.Uteis;

namespace Entidade
{
    public class LayoutFormato : BaseEntity
    {
        public virtual string Descricao { get; set; }

        public virtual FormatoExportacao Formato { get; set; }
        
        public virtual string Delimitador { get; set; }
        
        public virtual IList<LayoutLinha> Linhas { get; set; }

        public virtual Layout Layout { get; set; }

    }
}