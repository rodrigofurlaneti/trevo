using System;

namespace Aplicacao.ViewModels
{
    public class DescontoTipoViewModel
    {
        public string Nome { get; set; }
        public bool IsPercentual { get; set; }
        public bool IsValorFixo { get; set; }
        public bool IsHora { get; set; }
        public decimal Valor { get; set; }
        public int TabelaId { get; set; }
        public decimal ValorFaturamento { get; set; }
        public int UnidadeId { get; set; }
        public int Id { get; set; }
        public DateTime DataInsercao { get; set; }
    }
}