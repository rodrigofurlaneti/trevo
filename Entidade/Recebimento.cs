using Entidade.Base;
using Entidade.Uteis;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Entidade
{
    public class Recebimento : BaseEntity, IAudit
    {
        [Required]
        public virtual IList<Pagamento> Pagamentos { get; set; }

        public virtual StatusRecebimento? StatusRecebimento { get; set; }

        public virtual IList<LancamentoCobranca> LancamentosCobranca { get; set; }

        //public virtual IList<DescontoPagamento> DescontosPagamento { get; set; }

        public Recebimento()
        {
            Pagamentos = new List<Pagamento>();
            StatusRecebimento = new Uteis.StatusRecebimento();
            LancamentosCobranca = new List<LancamentoCobranca>();
        }
    }
}
