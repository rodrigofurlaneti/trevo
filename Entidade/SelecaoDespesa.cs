using Entidade.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidade
{
    public class SelecaoDespesa : BaseEntity
    {
        public virtual int MesVigente { get; set; }
        public virtual int Ano { get; set; }
        public virtual Unidade Unidade { get; set; }
        public virtual Empresa Empresa { get; set; }
        public virtual IList<DespesaContasAPagar> Despesas { get; set; }
    }
}
