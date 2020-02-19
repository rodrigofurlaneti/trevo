using System;
using Entidade;

namespace Aplicacao.ViewModels
{
    public class EnderecoViewModel
    {
        public int Id { get; set; }
        public DateTime DataInsercao { get; set; }
        public string Cep { get; set; }
        public string CidadeDescricao { get; set; }
        public CidadeViewModel Cidade { get; set; }
        public string Estado { get; set; }
        public string Bairro { get; set; }
        public string Numero { get; set; }
        public string Complemento { get; set; }
        public string Logradouro { get; set; }
        public string Tipo { get; set; }
        public string UF { get; set; }

        public string Resumo => $"{Logradouro}, {Numero} {(!string.IsNullOrWhiteSpace(Complemento) ? " - " + Complemento : string.Empty)} - {Cep} {(!string.IsNullOrWhiteSpace(CidadeDescricao) ? " - " + Cidade : string.Empty)}";

        public EnderecoViewModel()
        {
            DataInsercao = DateTime.Now;
        }

        public EnderecoViewModel(Endereco endereco)
        {
            Id = endereco?.Id ?? 0;
            Bairro = endereco?.Bairro;
            Cep = endereco?.Cep;
            Cidade = new CidadeViewModel(endereco?.Cidade);
            CidadeDescricao = endereco?.Cidade?.Descricao;
            Complemento = endereco?.Complemento;
            Logradouro = endereco?.Logradouro;
            Numero = endereco?.Numero;
            DataInsercao = endereco?.DataInsercao ?? DateTime.Now;
            Tipo = endereco?.Tipo;
            UF = endereco?.UF;
        }

        public Endereco ToEntity() => new Endereco
        {
            Bairro = this.Bairro,
            Cep = this.Cep,
            Cidade = this.RetornarCidade(),
            Complemento = this.Complemento,
            DataInsercao = this.DataInsercao < System.Data.SqlTypes.SqlDateTime.MinValue.Value ? DateTime.Now : this.DataInsercao,
            Logradouro = this.Logradouro,
            Id = this.Id,
            Numero = this.Numero,
            Tipo = this.Tipo,
            UF = this.UF
        };
        
        private Cidade RetornarCidade()
        {
			return string.IsNullOrEmpty(Cidade?.Descricao) ? 
                new Cidade() { Descricao = CidadeDescricao, Estado = new Estado { Sigla = string.IsNullOrEmpty(Cidade?.Estado?.Sigla) ? UF : Cidade.Estado.Sigla } } 
                : Cidade.ToEntity();
		}
    }
}