using Entidade;
using System;

namespace Aplicacao.ViewModels
{
    public class TabelaPrecoMensalistaUnidadeViewModel
    {
        public int Id { get; set; }
        public Unidade Unidade { get; set; }

        public string HorarioInicio { get; set; }
        public string HorarioFim { get; set; }

        public bool HoraAdicional { get; set; }
        public int QuantidadeHoras { get; set; }
        public string ValorQuantidade { get; set; }

        public int DiasParaCorte { get; set; }

        public TabelaPrecoMensalistaUnidadeViewModel()
        {


        }

        public TabelaPrecoMensalistaUnidadeViewModel(TabelaPrecoMensalistaUnidade TabelaPrecoMensalistaUnidade)
        {
            Id = TabelaPrecoMensalistaUnidade.Id;
            Unidade = TabelaPrecoMensalistaUnidade.Unidade;
            HorarioInicio = TabelaPrecoMensalistaUnidade.HorarioInicio;
            HorarioFim = TabelaPrecoMensalistaUnidade.HorarioFim;
            HoraAdicional = TabelaPrecoMensalistaUnidade.HoraAdicional;
            QuantidadeHoras = TabelaPrecoMensalistaUnidade.QuantidadeHoras;
            ValorQuantidade = TabelaPrecoMensalistaUnidade.ValorQuantidade.ToString();
            DiasParaCorte = TabelaPrecoMensalistaUnidade.DiasParaCorte;
        }

        public TabelaPrecoMensalistaUnidade ToEntity()
        {
            var entidade = new TabelaPrecoMensalistaUnidade
            {
                Id = Id,
                Unidade = Unidade,
                HorarioInicio = HorarioInicio,
                HorarioFim = HorarioFim,
                HoraAdicional = HoraAdicional,
                QuantidadeHoras = QuantidadeHoras,
                ValorQuantidade = string.IsNullOrEmpty(ValorQuantidade) ? 0 : Convert.ToDecimal(ValorQuantidade.Replace(".", "")),
                DiasParaCorte = DiasParaCorte
            };

            return entidade;
        }
    }
}


