using Entidade;
using Entidade.Uteis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Aplicacao.ViewModels
{
    public class EstoqueViewModel
    {
        public int Id { get; set; }

        public DateTime DataInsercao { get; set; }
        public string Nome { get; set; }
        public string Cep { get; set; }
        public string Logradouro { get; set; }
        public string Numero { get; set; }
        public string Complemento { get; set; }
        public string Bairro { get; set; }
        public TipoEndereco Tipo { get; set; }
        public string CidadeNome { get; set; }
        public string UF { get; set; }
        public string CidadeDescricao { get; set; }
        public bool EstoquePrincipal { get; set; }

        public UnidadeViewModel Unidade { get; set; }

        public EstoqueViewModel()
        {
            DataInsercao = DateTime.Now;
        }

        public EstoqueViewModel(Estoque estoque)
        {
            if (estoque != null)
            {
                Id = estoque.Id;
                DataInsercao = estoque.DataInsercao;
                Nome = estoque.Nome;
                Cep = estoque.Cep;
                Logradouro = estoque.Logradouro;
                Numero = estoque.Numero;
                Complemento = estoque.Complemento;
                Bairro = estoque.Bairro;
                Tipo = estoque.Tipo;
                CidadeNome = estoque.CidadeNome;
                UF = estoque.UF;
                EstoquePrincipal = estoque.EstoquePrincipal;

                Unidade = new UnidadeViewModel(estoque.Unidade); 
            }
        }

        public Estoque ToEntity()
        {
            var entidade = new Estoque
            {
                Id = Id,
                DataInsercao = this.DataInsercao < System.Data.SqlTypes.SqlDateTime.MinValue.Value ? DateTime.Now : this.DataInsercao,
                Nome = Nome,
                Cep = Cep,
                Logradouro = Logradouro,
                Numero = Numero,
                Complemento = Complemento,
                Bairro = Bairro,
                Tipo = Tipo,
                CidadeNome = CidadeNome,
                UF = UF,
                EstoquePrincipal = EstoquePrincipal,
                Unidade = Unidade != null ? Unidade.ToEntity() : new Unidade()
            };

            return entidade;
        }

        //private Cidade RetornarCidade()
        //{
        //    var cidadeNome = this.Cidade?.Descricao ?? this.CidadeDescricao;
        //    return !string.IsNullOrEmpty(cidadeNome) ? new Cidade() { Descricao = cidadeNome } : null;
        //}
    }
}
