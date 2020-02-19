using Entidade.Uteis;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;

namespace API.Models
{
    public class ResourceQueryPagamentos
    {
        public DateTime? DataPagamento { get; set; }
        public DateTime? DataEnvio { get; set; }
        public StatusPagamento statusPagamento { get; set; }
    }

    public class ResourceQueryRecebimentos
    {
        public StatusRecebimento? StatusRecebimento { get; set; }
        public DateTime? DataVencimento { get; set; }
    }

    public class ResourceParametersFaturamento
    {
        [Required]
        public int CodUnidade { get; set; }
        public string NomeUnidade { get; set; }
        [Required]
        public int NumFechamento { get; set; }
        [Required]
        public int NumTerminal { get; set; }
        [Required]
        public DateTime? DataAbertura { get; set; }
        [Required]
        public DateTime? DataFechamento { get; set; }
        [Required]
        public string TicketInicial { get; set; }
        [Required]
        public string TicketFinal { get; set; }
        [Required]
        public string PatioAtual { get; set; }
        [Required]
        public decimal ValorTotal { get; set; }
        public decimal? ValorRotativo { get; set; }
        public decimal? ValorRecebimentoMensalidade { get; set; }
        public decimal? ValorDinheiro { get; set; }
        public decimal? ValorCartaoDebito { get; set; }
        public decimal? ValorCartaoCredito { get; set; }
        public decimal? ValorSemParar { get; set; }
        public decimal? ValorSeloDesconto { get; set; }
    }



    public class ResourceParametersCliente
    {
        [Required]
        public string Nome { get; set; }
        [Required]
        public  string Sexo { get; set; }
        [Required]
        public  string Cpf { get; set; }
        [Required]
        public  string Rg { get; set; }
        [Required]
        public  string Cep { get; set; }
        public string Logradouro { get; set; }
        public string NumeroEndereco { get; set; }
        public string Complemento { get; set; }
        public string Bairro { get; set; }
        public string Cidade { get; set; }
        [Required]
        public  string Telefone { get; set; }
        [Required]
        public  string MarcaVeiculo { get; set; }
        [Required]
        public  string ModeloVeiculo { get; set; }
        [Required]
        public  string TipoVeiculo { get; set; }
        [Required]
        public string PlacaVeiculo { get; set; }
        [Required]
        public string CorVeiculo { get; set; }
        [Required]
        public int AnoVeiculo { get; set; }
    }


    public class ResourceParametersMovimentacao
    {
        [Required]
        public int CodUnidade { get; set; }
        public string NomeUnidade { get; set; }
        [Required]
        public int NumFechamento { get; set; }
        [Required]
        public int NumTerminal { get; set; }
        [Required]
        public DateTime? DataAbertura { get; set; }
        [Required]
        public DateTime? DataFechamento { get; set; }
        [Required]
        public string Ticket { get; set; }
        [Required]
        public string Placa { get; set; }
        [Required]
        public DateTime? DataEntrada { get; set; }
        [Required]
        public DateTime? DataSaida { get; set; }
        [Required]
        public decimal ValorCobrado { get; set; }
        [Required]
        public int TipoCliente { get; set; }
        public string NumeroContrato { get; set; }
        public int DescontoUtilizado { get; set; }
        public decimal ValorDesconto { get; set; }

    }

    public class ResourceParametersPagamentoMensalidade
    {
        [Required]
        public int CodUnidade { get; set; }
        public string NomeUnidade { get; set; }
        [Required]
        public int NumFechamento { get; set; }
        [Required]
        public int NumTerminal { get; set; }
        [Required]
        public DateTime? DataAbertura { get; set; }
        [Required]
        public DateTime? DataFechamento { get; set; }
        [Required]
        public DateTime? DataRecebimento { get; set; }
        public string NumContratoMensalista { get; set; }
        [Required]
        public decimal ValorRecebido { get; set; }
        public int NumCobranca { get; set; }

    }

}