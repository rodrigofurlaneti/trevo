using Entidade.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidade
{
    public class ImportacaoPagamento : BaseEntity
    {
        public virtual string Arquivo { get; set; }
        public virtual int Lote { get; set; }
        public virtual DateTime DataPagamento { get; set; }
        public virtual Usuario Usuario { get; set; }
        public virtual string Cedente { get; set; }
        public virtual IList<Pagamento> Pagamento { get; set; }

        public ImportacaoPagamento()
        {
            DataPagamento = System.Data.SqlTypes.SqlDateTime.MinValue.Value;
        }
    }
}
