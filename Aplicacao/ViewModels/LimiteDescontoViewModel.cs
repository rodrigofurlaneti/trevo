using Entidade;
using Entidade.Uteis;
using System;
using System.Globalization;

namespace Aplicacao.ViewModels
{
    public class LimiteDescontoViewModel
    {
        public int Id { get; set; }
        public DateTime DataInsercao { get; set; }
        public TipoServico TipoServico { get; set; }
        public TipoValor TipoValor { get; set; }
        public string Valor { get; set; }

        public LimiteDescontoViewModel()
        {
            DataInsercao = DateTime.Now;
        }

        public LimiteDescontoViewModel(LimiteDesconto limiteDesconto)
        {
            Id = limiteDesconto.Id;
            DataInsercao = limiteDesconto.DataInsercao;
            TipoServico = limiteDesconto.TipoServico;
            TipoValor = limiteDesconto.TipoValor;
            Valor = limiteDesconto.Valor.ToString("0.##");
        }

        public LimiteDesconto ToEntity()
        {
            return new LimiteDesconto
            {
                Id = Id,
                DataInsercao = DataInsercao,
                TipoServico = TipoServico,
                TipoValor = TipoValor,
                Valor = decimal.Parse(Valor, CultureInfo.InvariantCulture)
            };
        }

        public LimiteDescontoViewModel ToViewModel(LimiteDesconto limiteDesconto)
        {
            return new LimiteDescontoViewModel
            {
                Id = limiteDesconto.Id,
                DataInsercao = limiteDesconto.DataInsercao,
                TipoServico = limiteDesconto.TipoServico,
                TipoValor = limiteDesconto.TipoValor,
                Valor = limiteDesconto.Valor.ToString("0.##")
            };
            
        }

    }
}
