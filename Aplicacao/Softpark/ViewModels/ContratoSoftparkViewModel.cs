using Aplicacao.Softpark.ViewModels;
using Entidade;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Aplicacao.ViewModels
{
    /// <summary>
    /// Classe referente a ContratoMensalista
    /// </summary>
    public class ContratoSoftparkViewModel : BaseSoftparkViewModel
    {
        public int NumeroVagas { get; set; }
        public decimal ValorMensalidade { get; set; }
        public string Contrato { get; set; }
        public DateTime DataValidade { get; set; }
        public int DiaPgto { get; set; }
        public string HoraEntrada { get; set; }
        public string HoraSaida { get; set; }
        public string MinutoEntrada { get; set; }
        public string MinutoSaida { get; set; }
        public bool InAtivo { get; set; }
        public int CondutorId { get; set; }
        public IList<ContratoMensalistaCarroSoftparkViewModel> Carros { get; set; }
        public EstacionamentoSoftparkViewModel Estacionamento { get; set; }
        public TabelaPrecoMensalSoftparkViewModel TabelaPrecoMensal { get; set; }
        public Boolean IsFrota { get; set; }

        public ContratoSoftparkViewModel()
        {
        }

        public ContratoSoftparkViewModel(ContratoMensalista contratoMensalista)
        {
            Id = contratoMensalista.Id;
            DataInsercao = contratoMensalista.DataInsercao;
            NumeroVagas = contratoMensalista.NumeroVagas;
            ValorMensalidade = contratoMensalista.Valor;
            Contrato = contratoMensalista.NumeroContrato.ToString();
            DataValidade = contratoMensalista.DataVencimento;
            DiaPgto = contratoMensalista.DataVencimento.Day;
            HoraEntrada = "00";
            HoraSaida = "23";
            MinutoEntrada = "00";
            MinutoSaida = "00";
            InAtivo = contratoMensalista.Ativo;
            CondutorId = contratoMensalista.Cliente.Id;
            Carros = contratoMensalista?.Veiculos != null && contratoMensalista.Veiculos.Any() 
                        ? contratoMensalista?.Veiculos?.Select(x => new ContratoMensalistaCarroSoftparkViewModel(this, new CarroSoftparkViewModel(x.Veiculo)))?.ToList()
                        : null;
            Estacionamento = contratoMensalista.Unidade != null ? new EstacionamentoSoftparkViewModel(contratoMensalista.Unidade) : new EstacionamentoSoftparkViewModel();
            TabelaPrecoMensal = contratoMensalista.TabelaPrecoMensalista != null ? new TabelaPrecoMensalSoftparkViewModel(contratoMensalista.TabelaPrecoMensalista) : new TabelaPrecoMensalSoftparkViewModel();
            IsFrota = contratoMensalista.Frota;
        }

        public ContratoMensalista ToContratoMensalista(Cliente cliente, List<Veiculo> veiculos)
        {
            var contrato = new ContratoMensalista
            {
                Id = 0,
                DataInsercao = DataInsercao,
                Ativo = InAtivo,
                Cliente = cliente,
                DataInicio = DataInsercao,
                DataVencimento = DataValidade,
                NumeroContrato = int.TryParse(Contrato, out int result) ? result : 0,
                NumeroVagas = NumeroVagas,
                Valor = ValorMensalidade,
                Frota = IsFrota
            };

            contrato.Veiculos = veiculos?.Select(x => new ContratoMensalistaVeiculo
            {
                DataInsercao = DateTime.Now,
                ContratoMensalista = contrato,
                Veiculo = x
            }).ToList() ?? new List<ContratoMensalistaVeiculo>();

            return contrato;
        }
    }
}
