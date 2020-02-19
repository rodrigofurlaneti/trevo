using Entidade;
using Entidade.Uteis;
using System;

namespace Aplicacao.ViewModels
{
    public class ControleCompraViewModel
    {
        public int Id { get; set; }
        public DateTime DataInsercao { get; set; }
        public DateTime? DataServico { get; set; }
        public StatusCompraServico StatusCompraServico { get; set; }
        public DateTime? NovaData { get; set; }
        public OrcamentoSinistroCotacaoViewModel OrcamentoSinistroCotacao { get; set; }
        public PecaServicoViewModel PecaServico { get; set; }
        public string Observacao { get; set; }
    }
}