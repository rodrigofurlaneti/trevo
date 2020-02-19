using Entidade;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web.Mvc;

namespace Aplicacao.ViewModels
{
    public class ContratoMensalistaViewModel
    {
        public int Id { get; set; }
        public DateTime DataInsercao { get; set; }
        public ClienteViewModel Cliente { get; set; }
        public TipoMensalistaViewModel TipoMensalista { get; set; }
        public UnidadeViewModel Unidade { get; set; }
        public DateTime DataInicio { get; set; }
        public DateTime DataVencimento { get; set; }
        public string Valor { get; set; }
        public string ValorReais { get; set; }
        public int NumeroVagas { get; set; }
        public bool Frota { get; set; }
        [DisplayFormat(DataFormatString = "{0:dd MMM yyyy}")]
        public DateTime? DataFim { get; set; }
        public bool Ativo { get; set; }
        public int NumeroContrato { get; set; }
        public string UnidadeSelecionada { get; set; }

        public string HorarioFim { get; set; }
        public string HorarioInicio { get; set; }

        [AllowHtml]
        public string Observacao { get; set; }

        public string NumeroRecibo { get; set; }
        public decimal ValorPago { get; set; }

        public IList<VeiculoViewModel> Veiculos { get; set; }
        
        public TabelaPrecoMensalistaViewModel TabelaPrecoMensalista { get; set; }

        public bool PagamentoCadastro { get; set; }

        public ContratoMensalistaViewModel()
        {
            DataInsercao = DateTime.Now;
            Cliente = new ClienteViewModel();
            TipoMensalista = new TipoMensalistaViewModel();
            Unidade = new UnidadeViewModel();
            Veiculos = new List<VeiculoViewModel>();
            TabelaPrecoMensalista = new TabelaPrecoMensalistaViewModel();
        }

        public ContratoMensalistaViewModel(ContratoMensalista contratoMensalista)
        {
	        if (contratoMensalista != null)
	        {
		        Id = contratoMensalista.Id;
		        DataInsercao = contratoMensalista.DataInsercao;
		        Ativo = contratoMensalista.Ativo;
				Cliente = new ClienteViewModel(contratoMensalista.Cliente);
		        DataFim = contratoMensalista.DataFim;
		        DataInicio = contratoMensalista.DataInicio;
		        DataVencimento = contratoMensalista.DataVencimento;
		        NumeroContrato = contratoMensalista.NumeroContrato;
				TipoMensalista = new TipoMensalistaViewModel(contratoMensalista.TipoMensalista);
		        Unidade = new UnidadeViewModel(contratoMensalista.Unidade);
		        Veiculos = contratoMensalista?.Veiculos?.Select(x => new VeiculoViewModel(x.Veiculo))?.ToList();
		        Valor = contratoMensalista.Valor.ToString("N2");
		        ValorReais = contratoMensalista.Valor.ToString("C");
                NumeroVagas = contratoMensalista.NumeroVagas;
                TabelaPrecoMensalista = contratoMensalista?.TabelaPrecoMensalista != null ? new TabelaPrecoMensalistaViewModel(contratoMensalista?.TabelaPrecoMensalista) : null;
                Frota = contratoMensalista.Frota;
                HorarioInicio = contratoMensalista.HorarioInicio;
                HorarioFim = contratoMensalista.HorarioFim;
                Observacao = contratoMensalista.Observacao;
                NumeroRecibo = contratoMensalista.NumeroRecibo;
                ValorPago = contratoMensalista.ValorPago;
            }
        }

        public ContratoMensalista ToEntity()
        {
            return new ContratoMensalista
            {
                Id = this.Id,
                DataInsercao = this.DataInsercao,
                Ativo = this.Ativo,
                Cliente = this.Cliente == null ? null : this.Cliente?.ToEntity(),
                DataFim = this.DataFim,
                DataInicio = this.DataInicio,
                DataVencimento = this.DataVencimento,
                NumeroContrato = this.NumeroContrato,
                TipoMensalista = this.TipoMensalista?.ToEntity(),
                Unidade = this.Unidade == null ? null : this.Unidade?.ToEntity(),
                Veiculos = this.Veiculos == null || !this.Veiculos.Any() ? null : this.Veiculos?.Select(x => new ContratoMensalistaVeiculo { Veiculo = x?.ToEntity() })?.ToList(),
                Valor = Convert.ToDecimal(this.Valor),
                NumeroVagas = this.NumeroVagas,
                TabelaPrecoMensalista = this.TabelaPrecoMensalista == null ? null : this.TabelaPrecoMensalista?.ToEntity(),
                Frota = this.Frota,
                HorarioInicio = this.HorarioInicio,
                HorarioFim = this.HorarioFim,
                Observacao = this.Observacao,
                NumeroRecibo = this.NumeroRecibo,
                ValorPago = this.ValorPago
            };
        }
    }
}
