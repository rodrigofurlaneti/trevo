using Entidade;
using System;
using System.Collections.Generic;

namespace Aplicacao.ViewModels
{
    /// <summary>
    /// Classe referente a TabelaPrecoAvulso
    /// </summary>
    public class TabelaPrecoSoftparkViewModel : BaseSoftparkViewModel
    {
        public int Numero { get; set; }

        public string Nome { get; set; }

        public decimal ToleranciaInicial { get; set; }

        public decimal ToleranciaPgto { get; set; }

        public decimal QtdeHoraAdicional { get; set; }

        public decimal ValorHoraAdicional { get; set; }
        
        public bool Ativo { get; set; }
        
        public string DizeresImpressoTicket { get; set; }

        public bool IsBanco { get; set; }

        public bool IsLavagem { get; set; }

        public bool IsLoop { get; set; }

        public int IsCupom { get; set; }

        public List<TabelaPrecoEstacionamentoSoftparkViewModel> TabelaPrecoEstacionamento { get; set; }

        public IEnumerable<TabelaPrecoItemSoftparkViewModel> Items { get; set; }
        public IEnumerable<TabelaPrecoConfiguracaoDiariaSoftparkViewModel> ConfiguracaoDiaria { get; set; }

        public bool IsDefault { get; set; }

        public TabelaPrecoSoftparkViewModel()
        {
        }

        public TabelaPrecoSoftparkViewModel(TabelaPrecoAvulso tabelaPrecoAvulso)
        {
            Id = tabelaPrecoAvulso.Id;

            //var tempoInicial = TimeSpan.Parse(tabelaPrecoAvulso.HoraInicioVigencia);
            //var tempoFinal = TimeSpan.Parse(tabelaPrecoAvulso.HoraFimVigencia);

            var itens = new List<TabelaPrecoItemSoftparkViewModel>();
            foreach (var horaValor in tabelaPrecoAvulso.ListaHoraValor)
            {
                var tempoHoraValor = TimeSpan.Parse(horaValor.Hora);
                itens.Add(
                    new TabelaPrecoItemSoftparkViewModel
                    {
                        Id = horaValor.Id,
                        Hora = tempoHoraValor.Hours,
                        Minuto = tempoHoraValor.Minutes,
                        Valor = horaValor.Valor,
                        Ativo = true,
                        TabelaPrecoId = tabelaPrecoAvulso.Id,
                        DataInsercao = horaValor.DataInsercao
                    }
                );

            }

            var configuracaoDiaria = new List<TabelaPrecoConfiguracaoDiariaSoftparkViewModel>();
            foreach (var periodo in tabelaPrecoAvulso.ListaPeriodo)
            {
                configuracaoDiaria.Add(
                    new TabelaPrecoConfiguracaoDiariaSoftparkViewModel
                    {
                        Id = periodo.Id,
                        DiaSemana = (int)periodo.Periodo,
                        HoraInicial = TimeSpan.Parse(tabelaPrecoAvulso.HoraInicioVigencia).Hours,
                        HoraFinal = TimeSpan.Parse(tabelaPrecoAvulso.HoraFimVigencia).Hours,
                        TabelaPrecoId = tabelaPrecoAvulso.Id,
                        DataInsercao = periodo.DataInsercao
                    }
                );
            }

            var tabelaPrecoEstacionamentos = new List<TabelaPrecoEstacionamentoSoftparkViewModel>();
            foreach (var tabelaPrecoAvulsoUnidade in tabelaPrecoAvulso.ListaUnidade)
            {
                var estacionamento = new EstacionamentoSoftparkViewModel(tabelaPrecoAvulsoUnidade.Unidade);
                tabelaPrecoEstacionamentos.Add(new TabelaPrecoEstacionamentoSoftparkViewModel
                {
                    Id = tabelaPrecoAvulsoUnidade.Id,
                    TabelaPreco = this,
                    TabelaPrecoId = this.Id,
                    Estacionamento = estacionamento,
                    EstacionamentoId = estacionamento.Id,
                    DataInsercao = tabelaPrecoAvulsoUnidade.DataInsercao,
                    HoraInicial = TimeSpan.Parse(tabelaPrecoAvulsoUnidade.HoraInicio).Hours,
                    HoraFinal = TimeSpan.Parse(tabelaPrecoAvulsoUnidade.HoraFim).Hours,
                    MinutoInicial = TimeSpan.Parse(tabelaPrecoAvulsoUnidade.HoraInicio).Minutes,
                    MinutoFinal = TimeSpan.Parse(tabelaPrecoAvulsoUnidade.HoraFim).Minutes,
                    ValorMaximoDiaria = tabelaPrecoAvulsoUnidade.ValorDiaria
                });
            }

            Numero = tabelaPrecoAvulso.Numero;
            Nome = tabelaPrecoAvulso.Nome;
            ToleranciaInicial = tabelaPrecoAvulso.TempoToleranciaDesistencia;
            ToleranciaPgto = tabelaPrecoAvulso.TempoToleranciaPagamento;
            QtdeHoraAdicional = tabelaPrecoAvulso.QuantidadeHoraAdicional;
            ValorHoraAdicional = tabelaPrecoAvulso.ValorHoraAdicional;
            Ativo = true;
            DizeresImpressoTicket = tabelaPrecoAvulso.Nome;
            IsBanco = false;
            IsLavagem = false;
            IsLoop = false;
            IsCupom = 0;
            DataInsercao = tabelaPrecoAvulso.DataInsercao;
            Items = itens;
            ConfiguracaoDiaria = configuracaoDiaria;
            TabelaPrecoEstacionamento = tabelaPrecoEstacionamentos;
            IsDefault = tabelaPrecoAvulso.Padrao;
        }
    }
}
