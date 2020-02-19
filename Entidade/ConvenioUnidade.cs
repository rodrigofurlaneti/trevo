using Entidade.Base;
using System.Collections.Generic;

namespace Entidade
{
    public class ConvenioUnidade : BaseEntity
    {
        public virtual Unidade Unidade { get; set; }
        public virtual TipoSelo TipoSelo { get; set; }
        public virtual decimal Valor { get; set; }
    }
}