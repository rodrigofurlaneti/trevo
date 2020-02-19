using Entidade.Base;
using Entidade.Uteis;
using System;
using System.Collections.Generic;

namespace Entidade
{
    public class OrcamentoSinistroCotacaoItem : BaseEntity
    {
        public virtual OrcamentoSinistroCotacao OrcamentoSinistroCotacao { get; set; }
        public virtual Oficina Oficina { get; set; }
        public virtual Fornecedor Fornecedor { get; set; }
        public virtual PecaServico PecaServico { get; set; }
        public virtual ContasAPagar ContasAPagar { get; set; }
        public virtual LancamentoCobranca LancamentoCobranca { get; set; }
        public virtual FormaPagamentoOrcamentoSinistroCotacaoItem FormaPagamento { get; set; }
        public virtual int Quantidade { get; set; }
        public virtual decimal ValorUnitario { get; set; }
        public virtual decimal ValorTotal { get; set; }
        public virtual DateTime DataServico { get; set; }
        public virtual StatusCompraServico StatusCompraServico { get; set; }
        public virtual DateTime? NovaData { get; set; }
        public virtual bool TemSeguroReembolso { get; set; }
        public virtual string CiaSeguro { get; set; }
        public virtual DateTime? DataReembolso { get; set; }
        public virtual decimal ValorReembolso { get; set; }

        public virtual IList<OrcamentoSinistroCotacaoHistoricoDataItem> OrcamentoSinistroCotacaoHistoricoDataItens { get; set; }
    }
}