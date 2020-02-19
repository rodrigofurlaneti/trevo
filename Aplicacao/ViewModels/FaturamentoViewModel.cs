using Entidade;
using System;
using System.ComponentModel.DataAnnotations;

namespace Aplicacao.ViewModels
{
    public class FaturamentoViewModel : BaseSoftparkViewModel
    {
        public string NomeUnidade { get; set; }
        public int NumFechamento { get; set; }
        [Required(ErrorMessage = "o campo {0} é obrigatório")]
        public int NumTerminal { get; set; }
        [Required(ErrorMessage = "o campo {0} é obrigatório")]
        public DateTime DataAbertura { get; set; }
        [Required(ErrorMessage = "o campo {0} é obrigatório")]
        public DateTime DataFechamento { get; set; }
        public string TicketInicial { get; set; }
        public string TicketFinal { get; set; }
        [Required(ErrorMessage = "o campo {0} é obrigatório")]
        public string PatioAtual { get; set; }
        [Required(ErrorMessage = "o campo {0} é obrigatório")]
        public decimal? ValorTotal { get; set; }
        public decimal? ValorRotativo { get; set; }
        public decimal? ValorRecebimentoMensalidade { get; set; }
        public decimal? ValorDinheiro { get; set; }
        public decimal? ValorCartaoDebito { get; set; }
        public decimal? ValorCartaoCredito { get; set; }
        public decimal? ValorSemParar { get; set; }
        public decimal? ValorSeloDesconto { get; set; }
        public decimal? SaldoInicial { get; set; }
        public decimal? ValorSangria { get; set; }

        public virtual EstacionamentoSoftparkViewModel Estacionamento { get; set; }

        public virtual OperadorSoftparkViewModel Operador { get; set; }

        public FaturamentoViewModel()
        {
        }

        public FaturamentoViewModel(Faturamento faturamento)
        {
            Id = faturamento.IdSoftpark.HasValue && faturamento.IdSoftpark.Value > 0 ? faturamento.IdSoftpark.Value : faturamento.Id;
            DataInsercao = faturamento.DataInsercao;
            NomeUnidade = faturamento.NomeUnidade;
            NumFechamento = faturamento.NumFechamento;
            NumTerminal = faturamento.NumTerminal;
            DataAbertura = faturamento.DataAbertura;
            DataFechamento = faturamento.DataFechamento;
            TicketInicial = faturamento.TicketInicial;
            TicketFinal = faturamento.TicketFinal;
            PatioAtual = faturamento.PatioAtual;
            ValorTotal = faturamento.ValorTotal;
            ValorRotativo = faturamento.ValorRotativo;
            ValorRecebimentoMensalidade = faturamento.ValorRecebimentoMensalidade;
            ValorDinheiro = faturamento.ValorDinheiro;
            ValorCartaoDebito = faturamento.ValorCartaoDebito;
            ValorCartaoCredito = faturamento.ValorCartaoCredito;
            ValorSemParar = faturamento.ValorSemParar;
            ValorSeloDesconto = faturamento.ValorSeloDesconto;
            SaldoInicial = faturamento.SaldoInicial;
            ValorSangria = faturamento.ValorSangria;
            Estacionamento = faturamento.Unidade != null ? new EstacionamentoSoftparkViewModel(faturamento.Unidade) : null;
            Operador = faturamento.Usuario != null ? new OperadorSoftparkViewModel(faturamento.Usuario) : null;
        }

        public Faturamento ToEntity()
        {
            return new Faturamento
            {
                Id = 0,
                IdSoftpark = this.Id > 0 ? this.Id : default(int?),
                DataInsercao = this.DataInsercao,
                NomeUnidade = this.NomeUnidade,
                NumFechamento = this.NumFechamento,
                NumTerminal = this.NumTerminal,
                DataAbertura = this.DataAbertura,
                DataFechamento = this.DataFechamento,
                TicketInicial = this.TicketInicial,
                TicketFinal = this.TicketFinal,
                PatioAtual = this.PatioAtual,
                ValorTotal = this.ValorTotal,
                ValorRotativo = this.ValorRotativo,
                ValorRecebimentoMensalidade = this.ValorRecebimentoMensalidade,
                ValorDinheiro = this.ValorDinheiro,
                ValorCartaoDebito = this.ValorCartaoDebito,
                ValorCartaoCredito = this.ValorCartaoCredito,
                ValorSemParar = this.ValorSemParar,
                ValorSeloDesconto = this.ValorSeloDesconto,
                SaldoInicial = this.SaldoInicial,
                ValorSangria = this.ValorSangria,
            };
        }
    }
}
