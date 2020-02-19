using Entidade.Base;
using Entidade.Uteis;
using System.Collections.Generic;

namespace Entidade
{
    public class OrcamentoSinistroCotacao : BaseEntity
    {
        public virtual decimal ValorTotal { get; set; }
        public virtual int QuantidadeTotal { get; set; }
        public virtual IList<OrcamentoSinistroCotacaoItem> OrcamentoSinistroCotacaoItens { get; set; }
        public virtual IList<OrcamentoSinistroCotacaoNotificacao> OrcamentoSinistroCotacaoNotificacoes { get; set; }
        public virtual StatusOrcamentoSinistroCotacao Status { get; set; }

        public virtual void Aprovar()
        {
            Status = StatusOrcamentoSinistroCotacao.Aprovado;
        }

        public virtual void Negar()
        {
            Status = StatusOrcamentoSinistroCotacao.Negado;
        }
    }
}