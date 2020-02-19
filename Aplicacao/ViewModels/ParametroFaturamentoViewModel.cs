using Entidade;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;

namespace Aplicacao.ViewModels
{
    public class ParametroFaturamentoViewModel
    {
        public int Id { get; set; }
        public DateTime DataInsercao { get; set; }
        public EmpresaViewModel Empresa { get; set; }
        public string Descricao { get; set; }
        public BancoViewModel Banco { get; set; }
        public string Agencia { get; set; }
        public string DigitoAgencia { get; set; }
        public string Conta { get; set; }
        public string DigitoConta { get; set; }
        public decimal SaldoInicial { get; set; }
        public string Convenio { get; set; }
        public string Carteira { get; set; }
        public string CodigoTransmissao { get; set; }

        public string AgenciaDigito
        {
            get
            {
                return Agencia + " - " + DigitoAgencia;
            }
        }

        public string ContaDigito
        {
            get
            {
                return Conta + " - " + DigitoConta;
            }
        }

        public ParametroFaturamentoViewModel()
        {
            this.Empresa = new EmpresaViewModel();
            this.Banco = new BancoViewModel();
            DataInsercao = DateTime.Now;
        }

        public ParametroFaturamentoViewModel(ParametroFaturamento parametroFaturamento)
        {
            this.Id = parametroFaturamento?.Id ?? 0;
            this.DataInsercao = parametroFaturamento?.DataInsercao ?? DateTime.Now;
            this.Descricao = parametroFaturamento?.Descricao;
            this.Agencia = parametroFaturamento?.Agencia;
            this.DigitoAgencia = parametroFaturamento?.DigitoAgencia;
            this.Conta = parametroFaturamento?.Conta;
            this.DigitoConta = parametroFaturamento?.DigitoConta;
            this.SaldoInicial = parametroFaturamento?.SaldoInicial ?? 0;
            this.Convenio = parametroFaturamento?.Convenio;
            this.Carteira = parametroFaturamento?.Carteira;
            this.CodigoTransmissao = parametroFaturamento?.CodigoTransmissao;
            this.Empresa = AutoMapper.Mapper.Map<Empresa, EmpresaViewModel>(parametroFaturamento?.Empresa);
            this.Banco = new BancoViewModel(parametroFaturamento?.Banco);
        }

        public ParametroFaturamento ToEntity() => new ParametroFaturamento()
        {
            Id = this.Id,
            DataInsercao = this.DataInsercao < System.Data.SqlTypes.SqlDateTime.MinValue.Value ? DateTime.Now : this.DataInsercao,
            Descricao = this.Descricao,
            Agencia = this.Agencia,
            DigitoAgencia = this.DigitoAgencia,
            Conta = this.Conta,
            DigitoConta = this.DigitoConta,
            SaldoInicial = this.SaldoInicial,
            Convenio = this.Convenio,
            Carteira = this.Carteira,
            CodigoTransmissao = this.CodigoTransmissao,
            Empresa = AutoMapper.Mapper.Map<EmpresaViewModel, Empresa>(Empresa),
            Banco = this.Banco.ToEntity()
        };
    }
}
