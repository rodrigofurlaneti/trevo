using Entidade;
using System;

namespace Aplicacao.ViewModels
{
    public class FeriasClienteContaCorrenteViewModel
    {
        public int Id { get; set; }
        public DateTime DataInsercao { get; set; }
        public ContaCorrenteClienteViewModel ContaCorrente { get; set; }
        public FeriasClienteViewModel FeriasCliente { get; set; }

        public FeriasClienteContaCorrenteViewModel()
        {
            DataInsercao = DateTime.Now;
        }

        public FeriasClienteContaCorrenteViewModel(FeriasClienteContaCorrente entity)
        {
            Id = entity.Id;
            DataInsercao = entity.DataInsercao;
            ContaCorrente = new ContaCorrenteClienteViewModel(entity.ContaCorrente);
            FeriasCliente = new FeriasClienteViewModel { Id = FeriasCliente?.Id ?? 0 };
        }

        public FeriasClienteContaCorrente ToEntity() => new FeriasClienteContaCorrente
        {
            Id = this.Id,
            DataInsercao = this.DataInsercao,
            ContaCorrente = new ContaCorrenteCliente { Id = this.ContaCorrente?.ContaCorrenteClienteId ?? 0 },
            FeriasCliente = new FeriasCliente { Id = this.FeriasCliente?.Id ?? 0 }
        };
    }
}