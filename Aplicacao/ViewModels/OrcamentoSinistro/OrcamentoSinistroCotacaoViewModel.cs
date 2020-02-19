using Entidade.Base;
using System.Collections.Generic;
using Entidade.Uteis;
using System;

namespace Aplicacao.ViewModels
{
    public class OrcamentoSinistroCotacaoViewModel
    {
        public int Id { get; set; }
        public DateTime DataInsercao { get; set; }
        public decimal ValorTotal { get; set; }
        public int QuantidadeTotal { get; set; }
        public List<OrcamentoSinistroCotacaoItemViewModel> OrcamentoSinistroCotacaoItens { get; set; }
        public StatusOrcamentoSinistroCotacao Status { get; set; }
    }
}