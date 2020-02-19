using Entidade;
using Entidade.Uteis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace API.Models
{
    public class RecebimentoVM
    {
        public int Id { get; set; }
        public DateTime DataInsercao { get; set; }

        public IList<Pagamento> Pagamentos { get; set; }

        public StatusRecebimento? StatusRecebimento { get; set; }

        public IList<LancamentoCobranca> LancamentosCobranca { get; set; }

        public RecebimentoVM()
        {
            DataInsercao = DateTime.Now;
            Pagamentos = new List<Pagamento>();
            StatusRecebimento = new StatusRecebimento();
            LancamentosCobranca = new List<LancamentoCobranca>();
        }
    }
}