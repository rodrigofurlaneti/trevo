using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Entidade.Base;

namespace Entidade
{
    public class DespesaContasAPagar:BaseEntity
    {
        public virtual ContasAPagar ContaAPagar { get; set; }
        public virtual SelecaoDespesa SelecaoDespesa { get; set; }
    }
}
