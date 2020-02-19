using Entidade;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Aplicacao.ViewModels
{
    public class PropostaViewModel
    {
        public int Id { get; set; }
        public DateTime DataInsercao { get; set; }
        public ClienteViewModel Empresa { get; set; }
        public string Telefone { get; set; }
        public UnidadeViewModel Filial { get; set; }
        public string Endereco { get; set; }
        public string HorarioFuncionamento { get; set; }
        public string Email { get; set; }
        public List<string> ListaEmail => Email?.Split(',')?.ToList() ?? new List<string>();
        public string NumeroProposta { get; set; }
        public bool TemPedido { get; set; }
        public string Descricao
        {
            get
            {
                var empresa = string.IsNullOrEmpty(Empresa.Pessoa.Nome) ? Empresa.NomeFantasia : Empresa.Pessoa.Nome;
                var filial = Filial.Nome;

                return $"{Id} - {empresa} - {filial}";
            }
        }

        public PropostaViewModel()
        {
            Empresa = new ClienteViewModel();
            Filial = new UnidadeViewModel();
            TemPedido = false;
        }

        public PropostaViewModel(Proposta proposta)
        {   
            Id = proposta?.Id ?? 0;
            DataInsercao = proposta?.DataInsercao ?? DateTime.Now;
            Empresa = new ClienteViewModel(proposta?.Cliente) ?? new ClienteViewModel();
            Filial = new UnidadeViewModel(proposta?.Unidade) ?? new UnidadeViewModel();
            Email = proposta?.Email ?? string.Empty;
            TemPedido = false;
            NumeroProposta = (proposta?.Id ?? 0).ToString();
        }

        public Proposta ToEntity()
        {
            return new Proposta
            {
                Id = Id,
                DataInsercao = DataInsercao != DateTime.MinValue ? DataInsercao : DateTime.Now,
                Cliente = Empresa.ToEntity(),
                Unidade = Filial.ToEntity(),
                Email = Email
            };
        }
    }
}