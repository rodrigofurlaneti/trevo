using System.Collections.Generic;
using System.Linq;
using Entidade.Base;
using Entidade.Uteis;

namespace Entidade
{
    public class LayoutLinha : BaseEntity
    {
        public virtual string TipoLinha { get; set; }

        public virtual string CodigoLinha { get; set; }
        
        public virtual IList<LayoutCampo> Campos { get; set; }

        public virtual LayoutFormato LayoutFormato { get; set; }

        //Não Mapear
        public virtual int QuantidadeColunas => Campos == null || !Campos.Any() ? 0 : Campos.Count;
    }
}