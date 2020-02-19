using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Entidade;
using Entidade.Uteis;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;

namespace Aplicacao.ViewModels
{
    public class VeiculoViewModel
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "o campo {0} é obrigatório")]
        public string Placa { get; set; }
       
        public string Cor { get; set; }
        
        public int? Ano { get; set; }
        public DateTime DataInsercao { get; set; }
        [Required(ErrorMessage = "o campo {0} é obrigatório")]
        public ModeloViewModel Modelo { get; set; }
        [JsonConverter(typeof(StringEnumConverter))]
        [Required(ErrorMessage = "o campo {0} é obrigatório")]
        public TipoVeiculo TipoVeiculo { get; set; }
        public string TipoVeiculoDesc { get; set; }

        public string IdVeiculo { get; set; }
        public string VeiculoFull
        {
            get
            {
                return string.Format("{0} {1}, {2}", "Placa: " + Placa, "Modelo: " + Modelo?.Descricao, "Marca: " + Modelo?.Marca?.Nome);
            }
        }

        public VeiculoViewModel()
        {
            Modelo = new ModeloViewModel();
        }

        public VeiculoViewModel(Veiculo veiculo)
        {
            Id = veiculo?.Id ?? 0;
            Placa = veiculo?.Placa ?? string.Empty;
            Cor = veiculo?.Cor ?? string.Empty;
            Ano = veiculo?.Ano ?? 1900;
            DataInsercao = veiculo.DataInsercao;
            Modelo = new ModeloViewModel(veiculo?.Modelo ?? new Modelo());
            TipoVeiculo = veiculo.TipoVeiculo;
            if (TipoVeiculo > 0) TipoVeiculoDesc = GetTipoVeiculoDesc(TipoVeiculo); else TipoVeiculoDesc = "";
        }

        private string GetTipoVeiculoDesc(TipoVeiculo tipo) {
            return Enum.GetName(typeof(TipoVeiculo), tipo).ToString();
        }

        public Veiculo ToEntity()
        {
            return new Veiculo
            {
                Id = Id,
                Placa = Placa,
                Cor = Cor,
                Ano = Ano,
                DataInsercao = DataInsercao != DateTime.MinValue ? DataInsercao : DateTime.Now,
                Modelo = Modelo?.ToEntity(),
                TipoVeiculo = TipoVeiculo
            };
        }

        public VeiculoViewModel ToViewModel(Veiculo veiculo)
        {
            return new VeiculoViewModel
            {
                Id = veiculo.Id,
                Placa = veiculo.Placa,
                Cor = veiculo.Cor,
                Ano = veiculo.Ano,
                DataInsercao = veiculo.DataInsercao,
                Modelo = new ModeloViewModel(veiculo?.Modelo ?? new Modelo()),
                TipoVeiculo = veiculo.TipoVeiculo,
                TipoVeiculoDesc = GetTipoVeiculoDesc(veiculo.TipoVeiculo)
            };
        }

        public List<VeiculoViewModel> VeiculoViewModelList(IList<Veiculo> veiculos)
        {
            return veiculos.Select(veiculo => new VeiculoViewModel(veiculo)).ToList();
        }
    }
}
