using Entidade;
using Entidade.Uteis;
using System;

namespace Aplicacao.ViewModels
{
    /// <summary>
    /// Classe referente a Veiculo
    /// </summary>
    public class CarroSoftparkViewModel : BaseSoftparkViewModel
    {
        public string Placa { get; set; }
        public ModeloViewModel Modelo { get; set; }
        public string Cor { get; set; }
        public string Ano { get; set; }
        public DateTime DataGravacao { get; set; }
        public TipoVeiculo TipoVeiculo { get; set; }

        public CarroSoftparkViewModel()
        {
        }

        public CarroSoftparkViewModel(Veiculo veiculo)
        {
            Id = veiculo.Id;
            DataInsercao = veiculo.DataInsercao;
            DataGravacao = veiculo.DataInsercao;
            Ano = veiculo.Ano.ToString();
            Placa = veiculo.Placa;
            Cor = veiculo.Cor;
            Modelo = new ModeloViewModel(veiculo.Modelo);
            TipoVeiculo = veiculo.TipoVeiculo;
        }

        public ClienteVeiculo ToClienteVeiculo()
        {
            return new ClienteVeiculo
            {
                Id = 0,
                DataInsercao = DataInsercao,
                Veiculo = new Veiculo
                {
                    Id = 0,
                    DataInsercao = DataInsercao,
                    Ano = int.TryParse(Ano, out int result) ? result : 0,
                    Placa = Placa,
                    Cor = Cor,
                    Modelo = new Modelo { Id = Modelo.Id, Descricao = Modelo.Descricao },
                    TipoVeiculo = TipoVeiculo
                }
            };
        }
    }
}
