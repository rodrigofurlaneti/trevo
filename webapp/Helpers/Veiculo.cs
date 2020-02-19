using Aplicacao.ViewModels;
using System.Collections.Generic;

namespace Portal.Helpers
{
    public static class Veiculo
    {
        public static List<VeiculoViewModel> RetornaVeiculos(List<ClienteVeiculoViewModel> clienteVeiculos)
        {
            var veiculos = new List<VeiculoViewModel>();
            foreach(var clienteVeiculo in clienteVeiculos)
            {
                veiculos.Add(clienteVeiculo.Veiculo);
            }

            return veiculos;
        }


        public static List<ClienteVeiculoViewModel> RetornaClienteVeiculos(int cliente, List<VeiculoViewModel> veiculos)
        {
            var clienteVeiculos = new List<ClienteVeiculoViewModel>();
            foreach (var veiculo in veiculos)
            {
                var clienteVeiculo = new ClienteVeiculoViewModel();
                clienteVeiculo.Cliente = cliente;
                clienteVeiculo.Veiculo = new VeiculoViewModel();
                clienteVeiculo.Veiculo = veiculo;
                clienteVeiculos.Add(clienteVeiculo);
            }

            return clienteVeiculos;
        }
    }
}