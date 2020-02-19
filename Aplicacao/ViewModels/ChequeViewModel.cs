using System;
using Entidade;
using Entidade.Uteis;

namespace Aplicacao.ViewModels
{
    public class ChequeViewModel
    {
        public int Id { get; set; }
        public DateTime DataInsercao { get; set; }
        public ContaFinanceiraViewModel ContaFinanceira { get; set; }
        public string Emitente { get; set; }
        public string Agencia { get; set; }
        public string DigitoAgencia { get; set; }
        public string Conta { get; set; }
        public string DigitoConta { get; set; }
        public string Cpf { get; set; }
        public string Valor { get; set; }
        public DateTime? DataDevolucao { get; set; }
        public string MotivoDevolucao { get; set; }
        public DateTime? DataProtesto { get; set; }
        public string CartorioProtestado { get; set; }

        public StatusCheque StatusCheque { get; set; }
        public long Numero { get; set; }

        public BancoViewModel Banco { get; set; }

        public ChequeViewModel()
        {
            ContaFinanceira = new ContaFinanceiraViewModel();
            Banco = new BancoViewModel();
        }

        public ChequeViewModel(Cheque contaFinanceira)
        {
            Id = contaFinanceira.Id;
            DataInsercao = contaFinanceira.DataInsercao;
            Numero = contaFinanceira.Numero;
            Emitente = contaFinanceira.Emitente;
            Agencia = contaFinanceira.Agencia;
            DigitoAgencia = contaFinanceira.DigitoAgencia;
            Conta = contaFinanceira.Conta;
            DigitoConta = contaFinanceira.DigitoConta;
            Cpf = contaFinanceira.CPFCNPJ;
            Valor = contaFinanceira.Valor.ToString("0.00");

            DataDevolucao = contaFinanceira.DataDevolucao;
            MotivoDevolucao = contaFinanceira.MotivoDevolucao;

            DataProtesto = contaFinanceira.DataProtesto;
            CartorioProtestado = contaFinanceira.CartorioProtestado;

            StatusCheque = contaFinanceira.StatusCheque;
            ContaFinanceira = new ContaFinanceiraViewModel(contaFinanceira?.ContaFinanceira) ?? new ContaFinanceiraViewModel();
            Banco = new BancoViewModel(contaFinanceira?.Banco)??new BancoViewModel();
        }

        public Cheque ToEntity() => new Cheque
        {
            Id = Id,
            DataInsercao = DataInsercao < System.Data.SqlTypes.SqlDateTime.MinValue.Value ? DateTime.Now : this.DataInsercao,
            Numero = Numero,
            Emitente = Emitente,
            Agencia = Agencia,
            DigitoAgencia = DigitoAgencia,
            Conta = Conta,
            DigitoConta = DigitoConta,
            CPFCNPJ = Cpf,
            Valor = Convert.ToDecimal(Valor),

            DataDevolucao = DataDevolucao,
            MotivoDevolucao = MotivoDevolucao,

            DataProtesto = DataProtesto,
            CartorioProtestado = CartorioProtestado,

            StatusCheque = StatusCheque,
            ContaFinanceira = ContaFinanceira.ToEntity(),
            Banco = Banco.ToEntity()
        };
    }
}