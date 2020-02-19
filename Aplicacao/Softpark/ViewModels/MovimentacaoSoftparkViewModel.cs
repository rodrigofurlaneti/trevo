using Entidade;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Aplicacao.ViewModels
{
    public class MovimentacaoSoftparkViewModel : BaseSoftparkViewModel
    {
        public int NumFechamento { get; set; }
        [Required(ErrorMessage = "o campo {0} é obrigatório")]
        public int NumTerminal { get; set; }
        [Required(ErrorMessage = "o campo {0} é obrigatório")]
        public DateTime DataAbertura { get; set; }
        public DateTime DataFechamento { get; set; }
        public string Ticket { get; set; }
        [Required(ErrorMessage = "o campo {0} é obrigatório")]
        public string Placa { get; set; }
        [Required(ErrorMessage = "o campo {0} é obrigatório")]
        public DateTime DataEntrada { get; set; }
        [Required(ErrorMessage = "o campo {0} é obrigatório")]
        public DateTime DataSaida { get; set; }
        [Required(ErrorMessage = "o campo {0} é obrigatório")]
        public decimal ValorCobrado { get; set; }
        public string DescontoUtilizado { get; set; }
        [Required(ErrorMessage = "o campo {0} é obrigatório")]
        public decimal ValorDesconto { get; set; }
        [Required(ErrorMessage = "o campo {0} é obrigatório")]
        public string TipoCliente { get; set; }
        public string NumeroContrato { get; set; }
        [Required(ErrorMessage = "o campo {0} é obrigatório")]
        public int ClienteId { get; set; }
        public bool VagaIsenta { get; set; }
        public string Cpf { get; set; }
        public string Rps { get; set; }
        public string FormaPagamento { get; set; }

        public List<MovimentacaoSeloSoftparkViewModel> MovimentacaoSelo { get; set; }

        public virtual EstacionamentoSoftparkViewModel Estacionamento { get; set; }

        public virtual OperadorSoftparkViewModel Operador { get; set; }

        public MovimentacaoSoftparkViewModel()
        {
        }

        public MovimentacaoSoftparkViewModel(Movimentacao movimentacao)
        {
            Id = movimentacao.IdSoftpark.HasValue && movimentacao.IdSoftpark.Value > 0 ? movimentacao.IdSoftpark.Value : movimentacao.Id;
            DataInsercao = movimentacao.DataInsercao;
            NumFechamento = movimentacao.NumFechamento;
            NumTerminal = movimentacao.NumTerminal;
            DataAbertura = movimentacao.DataAbertura;
            DataFechamento = movimentacao.DataFechamento;
            Ticket = movimentacao.Ticket;
            Placa = movimentacao.Placa;
            DataEntrada = movimentacao.DataEntrada;
            DataSaida = movimentacao.DataSaida;
            ValorCobrado = movimentacao.ValorCobrado;
            DescontoUtilizado = movimentacao.DescontoUtilizado;
            ValorDesconto = movimentacao.ValorDesconto;
            TipoCliente = movimentacao.TipoCliente;
            NumeroContrato = movimentacao.NumeroContrato;
            ClienteId = movimentacao.Cliente.Id;
            VagaIsenta = movimentacao.VagaIsenta;
            Cpf = movimentacao.Cpf;
            Rps = movimentacao.Rps;
            FormaPagamento = movimentacao.FormaPagamento;
            MovimentacaoSelo = movimentacao.MovimentacaoSelo.Select(x => new MovimentacaoSeloSoftparkViewModel(x, this)).ToList();
            Estacionamento = movimentacao.Unidade != null ? new EstacionamentoSoftparkViewModel(movimentacao.Unidade) : null;
            Operador = movimentacao.Usuario != null ? new OperadorSoftparkViewModel(movimentacao.Usuario) : null;
        }

        public Movimentacao ToEntity()
        {
            var movimentacao = new Movimentacao();

            movimentacao.Id = 0;
            movimentacao.IdSoftpark = Id > 0 ? Id : default(int?);
            movimentacao.DataInsercao = DataInsercao;
            movimentacao.NumFechamento = NumFechamento;
            movimentacao.NumTerminal = NumTerminal;
            movimentacao.DataAbertura = DataAbertura;
            movimentacao.DataFechamento = DataFechamento;
            movimentacao.Ticket = Ticket;
            movimentacao.Placa = Placa;
            movimentacao.DataEntrada = DataEntrada;
            movimentacao.DataSaida = DataSaida;
            movimentacao.ValorCobrado = ValorCobrado;
            movimentacao.DescontoUtilizado = DescontoUtilizado;
            movimentacao.ValorDesconto = ValorDesconto;
            movimentacao.TipoCliente = TipoCliente;
            movimentacao.NumeroContrato = NumeroContrato;
            movimentacao.VagaIsenta = VagaIsenta;
            movimentacao.Cpf = Cpf;
            movimentacao.Rps = Rps;
            movimentacao.FormaPagamento = FormaPagamento;
            movimentacao.MovimentacaoSelo = MovimentacaoSelo?.Select(x => x.ToEntity()).ToList();

            return movimentacao;
        }

        public MovimentacaoSoftparkViewModel Clone()
        {
            return (MovimentacaoSoftparkViewModel)MemberwiseClone();
        }
    }
}
