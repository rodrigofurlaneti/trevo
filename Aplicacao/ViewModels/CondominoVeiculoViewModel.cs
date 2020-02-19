using Entidade;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Aplicacao.ViewModels
{
    public class CondominoVeiculoViewModel
    {
        public VeiculoViewModel Veiculo { get; set; }
        public ClienteCondominoViewModel Condomino { get; set; }

        public CondominoVeiculoViewModel()
        {
            Veiculo = new VeiculoViewModel();
        }

        public CondominoVeiculoViewModel(CondominoVeiculo condominoVeiculo)
        {
            Veiculo = new VeiculoViewModel(condominoVeiculo?.Veiculo ?? new Veiculo());
            Condomino = new ClienteCondominoViewModel(condominoVeiculo.Condomino);
        }

        public CondominoVeiculo ToEntity()
        {
            return new CondominoVeiculo
            {
                Veiculo = Veiculo?.ToEntity(),
                Condomino = Condomino.ToEntity()
            };
        }

        public CondominoVeiculo ToEntity(CondominoVeiculoViewModel condominoVeiculo)
        {
            return new CondominoVeiculo
            {
                DataInsercao = DateTime.Now,
                Veiculo = condominoVeiculo.Veiculo?.ToEntity(),
                Condomino = condominoVeiculo.Condomino.ToEntity()
            };
        }

        public CondominoVeiculoViewModel ToViewModel(CondominoVeiculo condominoVeiculo)
        {
            return new CondominoVeiculoViewModel
            {
                Veiculo = new VeiculoViewModel(condominoVeiculo?.Veiculo ?? new Veiculo()),
                Condomino = new ClienteCondominoViewModel(condominoVeiculo.Condomino)
            };
        }

        public IList<CondominoVeiculoViewModel> ListaVeiculos(IList<CondominoVeiculo> condominoVeiculos)
        {
            var condominoVeiculosViewModel = new List<CondominoVeiculoViewModel>();
            foreach (var veiculo in condominoVeiculos)
            {
                condominoVeiculosViewModel.Add(ToViewModel(veiculo));
            }

            return condominoVeiculosViewModel;
        }

        public IList<CondominoVeiculo> ListaVeiculos(IList<CondominoVeiculoViewModel> clienteVeiculos)
        {
            var condominoVeiculosViewModel = new List<CondominoVeiculo>();

            if (clienteVeiculos != null && clienteVeiculos.Any())
            {
                foreach (var veiculo in clienteVeiculos)
                {
                    condominoVeiculosViewModel.Add(ToEntity(veiculo));
                }
            }

            return condominoVeiculosViewModel;
        }
    }
}
