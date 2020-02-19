using System;
using System.Collections.Generic;
using System.Linq;
using Entidade;

namespace Aplicacao.ViewModels
{
    public class TrabalhoViewModel
    {
        public int Id { get; set; }
        public DateTime DataInsercao { get; set; }
        public string Empresa { get; set; }
        public List<TrabalhoContatoViewModel> Contatos { get; set; }
        public List<TrabalhoEnderecoViewModel> Enderecos { get; set; }
        public string Profissao { get; set; }
        public string Cargo { get; set; }

        //Mapeamento para Importacao Apenas
        public ContatoViewModel Contato { get; set; }
        public EnderecoViewModel Endereco { get; set; }

        public TrabalhoViewModel()
        {
            Contatos = new List<TrabalhoContatoViewModel>();
            Enderecos = new List<TrabalhoEnderecoViewModel>();
        }

        public TrabalhoViewModel(Trabalho entity)
        {
            this.Id = entity?.Id ?? 0;
            this.DataInsercao = entity?.DataInsercao ?? DateTime.Now;
            this.Empresa = entity?.Empresa;
            this.Contatos = entity?.Contatos?.Select(x => new TrabalhoContatoViewModel { Contato = new ContatoViewModel(x.Contato) }).ToList() ?? new List<TrabalhoContatoViewModel>();
            this.Enderecos = entity?.Enderecos?.Select(x => new TrabalhoEnderecoViewModel { Endereco = new EnderecoViewModel(x.Endereco) }).ToList() ?? new List<TrabalhoEnderecoViewModel>();
            this.Profissao = entity?.Profissao;
            this.Cargo = entity?.Cargo;

            this.Contato = this.Contatos?.FirstOrDefault()?.Contato ?? new ContatoViewModel();
            this.Endereco = this.Enderecos?.FirstOrDefault()?.Endereco ?? new EnderecoViewModel();
        }

        public Trabalho ToEntity() => string.IsNullOrEmpty(this.Empresa) ? null : new Trabalho
        {
            Id = this.Id,
            DataInsercao = this.DataInsercao,
            Empresa = this.Empresa,
            Contatos = this.RetornarContatos(),
            Enderecos = this.RetornarEnderecos(),
            Profissao = this.Profissao,
            Cargo = this.Cargo
        };

        private List<TrabalhoContato> RetornarContatos()
        {
            var contato = this?.Contatos ?? new List<TrabalhoContatoViewModel>();

            if (this.Contato != new ContatoViewModel() && !contato.Any(x => x.Contato != null && x.Contato.Numero == this.Contato.Numero || x.Contato != null && x.Contato.Email == this.Contato.Email))
                contato.Add(new TrabalhoContatoViewModel { Contato = this.Contato });

            return contato.Where(x => x.Contato != null).Select(x => new TrabalhoContato { Contato = x.Contato.ToEntity() }).ToList();
        }

        private List<TrabalhoEndereco> RetornarEnderecos()
        {
            var endereco = this?.Enderecos ?? new List<TrabalhoEnderecoViewModel>();

            if (this.Endereco != new EnderecoViewModel() && !endereco.Any(x => x.Endereco != null && x.Endereco.Logradouro == this.Endereco.Logradouro))
                endereco.Add(new TrabalhoEnderecoViewModel { Endereco = this.Endereco });

            return endereco.Where(x => x.Endereco != null).Select(x => new TrabalhoEndereco { Endereco = x.Endereco.ToEntity() }).ToList();
        }
    }

    public class TrabalhoContatoViewModel
    {
        public virtual ContatoViewModel Contato { get; set; }

        public TrabalhoContatoViewModel() { this.Contato = new ContatoViewModel(); }
    }

    public class TrabalhoEnderecoViewModel
    {
        public virtual EnderecoViewModel Endereco { get; set; }

        public TrabalhoEnderecoViewModel() { this.Endereco = new EnderecoViewModel(); } 
    }
}