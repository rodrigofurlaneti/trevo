using Entidade.Uteis;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

namespace Aplicacao.ViewModels
{
    public class OrcamentoSinistroCotacaoItemViewModel
    {
        [JsonIgnore]
        public OrcamentoSinistroCotacaoViewModel OrcamentoSinistroCotacao { get; set; }
        public OficinaViewModel Oficina { get; set; }
        public FornecedorViewModel Fornecedor { get; set; }
        public PecaServicoViewModel PecaServico { get; set; }
        public FormaPagamentoOrcamentoSinistroCotacaoItem FormaPagamento { get; set; }
        public int Quantidade { get; set; }
        public string ValorUnitario { get; set; }
        public string ValorTotal { get; set; }
        public string DataServico { get; set; }
        public StatusCompraServico StatusCompraServico { get; set; }
        public string NovaData { get; set; }
        public bool TemSeguroReembolso { get; set; }
        public string CiaSeguro { get; set; }
        public string DataReembolso { get; set; }
        public string ValorReembolso { get; set; }

        public virtual List<OrcamentoSinistroCotacaoHistoricoDataItemViewModel> OrcamentoSinistroCotacaoHistoricoDataItens { get; set; }
    }
}