using System.Collections.Generic;
using System.Linq;
using Entidade.Base;

namespace Entidade
{
    public class Layout : BaseEntity
    {
        public virtual string Nome { get; set; }
        
        public virtual IList<LayoutFormato> Formatos { get; set; }

        //Não Mapear
        public virtual int QuantidadeFormatos => Formatos == null || !Formatos.Any() ? 0 : Formatos.Count;
    }
}