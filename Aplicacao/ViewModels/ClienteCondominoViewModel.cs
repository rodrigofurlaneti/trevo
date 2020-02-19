using Entidade;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Aplicacao.ViewModels
{
    public class ClienteCondominoViewModel
    {
        public int Id { get; set; }
        public ClienteViewModel Cliente { get; set; }
        public UnidadeCondomino Unidade { get; set; }
        public int NumeroVagas { get; set; }
        public List<CondominoVeiculoViewModel> CondominoVeiculos { get; set; }
        public List<VeiculoViewModel> Veiculos { get; set; }
        public bool Frota { get; set; }
        public DateTime DataInsercao { get; set; }

        public ClienteCondominoViewModel()
        {
            DataInsercao = DateTime.Now;
        }

        public ClienteCondominoViewModel(ClienteCondomino clienteCondomino)
        {
            Id = clienteCondomino.Id;
            Cliente = new ClienteViewModel(clienteCondomino?.Cliente);
            Unidade = clienteCondomino.Unidade;
            NumeroVagas = clienteCondomino.NumeroVagas;
            DataInsercao = clienteCondomino.DataInsercao;
            Veiculos = clienteCondomino.CondominoVeiculos?.Select(x => new VeiculoViewModel(x.Veiculo)).ToList();
            Frota = clienteCondomino.Frota;
        }

        public ClienteCondomino ToEntity()
        {
            var clienteCondomino = new ClienteCondomino
            {
                Id = Id,
                Cliente = Cliente.ToEntity(),
                Unidade = Unidade,
                NumeroVagas = NumeroVagas,
                Frota = Frota,
                DataInsercao = DataInsercao
            };

            clienteCondomino.CondominoVeiculos = Veiculos?.Select(x => new CondominoVeiculo { Condomino = clienteCondomino, Veiculo = x.ToEntity() }).ToList();

            return clienteCondomino;
        }
    }
}
