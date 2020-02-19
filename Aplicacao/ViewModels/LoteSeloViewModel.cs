using System;

namespace Aplicacao.ViewModels
{
    public class LoteSeloViewModel
    {
        public int EtiquetaId { get; set; }
        public int ConvenioId { get; set; }
        public string ConvenioNome { get; set; }
        public DateTime Emissao { get; set; }
        public int Quantidade { get; set; }
        public DateTime DataVencimento { get; set; }
        public decimal ValorTotal { get; set; }
        public string Cobranca { get; set; }
        public string NumeracaoInicial { get; set; }
        public string NumeracaoFinal { get; set; }
        public int Ativo { get; set; }
        public string Vencimento { get; set; }
        public decimal ValorDesconto { get; set; }
        public int DescontoTipoId { get; set; }
        public int UnidadeId { get; set; }
        public int Id { get; set; }
        public DateTime DataInsercao { get; set; }
    }
}