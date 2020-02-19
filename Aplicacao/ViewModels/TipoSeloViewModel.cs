using Entidade;
using Entidade.Uteis;
using System;

namespace Aplicacao.ViewModels
{
    public class TipoSeloViewModel
    {
        public int Id { get; set; }
        public DateTime DataInsercao { get; set; }
        public string Nome { get; set; }
        public string Valor { get; set; }
        public bool ComValidade { get; set; }
        public bool PagarHorasAdicionais { get; set; }
        public bool Ativo { get; set; }
        public ParametroSelo ParametroSelo { get; set; }
        public string ParametroSeloFormatado { get; set; }

        public TipoSeloViewModel()
        {
        }

        public TipoSeloViewModel(TipoSelo tipoSelo)
        {
            if (tipoSelo != null)
            {
                Id = tipoSelo.Id;
                DataInsercao = tipoSelo.DataInsercao;
                Nome = tipoSelo.Nome;
                Valor = tipoSelo.Valor.ToString();
                ComValidade = tipoSelo.ComValidade;
                PagarHorasAdicionais = tipoSelo.PagarHorasAdicionais;
                Ativo = tipoSelo.Ativo;
                ParametroSelo = tipoSelo.ParametroSelo;
            }
        }

        public TipoSelo ToEntity()
        {
            return new TipoSelo
            {
                Id = Id,
                Nome = Nome,
                DataInsercao = DataInsercao,
                Ativo = Ativo,
                ComValidade = ComValidade,
                PagarHorasAdicionais = PagarHorasAdicionais,
                ParametroSelo = ParametroSelo,
                Valor = Convert.ToDecimal(Valor)
            };
        }
    }
}