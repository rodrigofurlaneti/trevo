using Entidade;
using Entidade.Uteis;
using System;
using System.Collections.Generic;

namespace Aplicacao.ViewModels
{
    public class RecebimentoViewModel
    {
        public int Id { get; set; }
        public DateTime DataInsercao { get; set; }

        public List<Pagamento> Pagamentos { get; set; }

        public StatusRecebimento? StatusRecebimento { get; set; }

        public List<LancamentoCobranca> LancamentosCobranca { get; set; }

        public RecebimentoViewModel()
        {
            DataInsercao = DateTime.Now;
            Pagamentos = new List<Pagamento>();
            StatusRecebimento = new StatusRecebimento();
            LancamentosCobranca = new List<LancamentoCobranca>();
        }
    }
}
