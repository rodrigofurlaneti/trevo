using Entidade;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;

namespace Aplicacao.ViewModels
{
    public class ClienteVeiculoViewModel
    {
        [Required(ErrorMessage = "o campo {0} é obrigatório")]
        public VeiculoViewModel Veiculo { get; set; }
        public int Cliente { get; set; }

        public ClienteVeiculoViewModel()
        {
            Veiculo = new VeiculoViewModel();
        }

        public ClienteVeiculoViewModel(ClienteVeiculo clienteVeiculo)
        {
            Veiculo = new VeiculoViewModel(clienteVeiculo?.Veiculo ?? new Veiculo());
            Cliente = clienteVeiculo.Cliente;
        }

        public ClienteVeiculo ToEntity()
        {
            return new ClienteVeiculo
            {
                Veiculo = Veiculo?.ToEntity(),
                Cliente = Cliente
            };
        }

        public ClienteVeiculo ToEntity(ClienteVeiculoViewModel clienteVeiculo)
        {
            return new ClienteVeiculo
            {
                DataInsercao = DateTime.Now,
                Veiculo = clienteVeiculo.Veiculo?.ToEntity(),
                Cliente = clienteVeiculo.Cliente
            };
        }

        public ClienteVeiculoViewModel ToViewModel(ClienteVeiculo clienteVeiculo)
        {
            return new ClienteVeiculoViewModel
            {
                Veiculo = new VeiculoViewModel(clienteVeiculo?.Veiculo ?? new Veiculo()),
                Cliente = clienteVeiculo.Cliente
            };
        }

        public IList<ClienteVeiculoViewModel> ListaVeiculos(IList<ClienteVeiculo> clienteVeiculos)
        {
            if (clienteVeiculos == null)
                return null;

            var clienteVeiculosViewModel = new List<ClienteVeiculoViewModel>();
            foreach (var veiculo in clienteVeiculos)
            {
                clienteVeiculosViewModel.Add(ToViewModel(veiculo));
            }

            return clienteVeiculosViewModel;
        }

        public IList<ClienteVeiculo> ListaVeiculos(IList<ClienteVeiculoViewModel> clienteVeiculos)
        {
            var clienteVeiculosViewModel = new List<ClienteVeiculo>();

            if (clienteVeiculos != null && clienteVeiculos.Any())
            {
                foreach (var veiculo in clienteVeiculos)
                {
                    clienteVeiculosViewModel.Add(ToEntity(veiculo));
                }
            }

            return clienteVeiculosViewModel;
        }
    }
}
