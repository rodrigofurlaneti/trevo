using Entidade.Uteis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacao.ViewModels
{
    public class RemanejamentoViewModel
    {
        public int Id { get; set; }
        public DateTime DataInsercao { get; set; }
        public bool Fixo { get; set; }
        public TipoOpreracao TipoOpreracao { get; set; }
        public DateTime? DataFim { get; set; }
        public RemanejamentoTransferenciaViewModel RemanejamentoOrigem { get; set; }
        public RemanejamentoTransferenciaViewModel RemanejamentoDestino { get; set; }
    }
}
