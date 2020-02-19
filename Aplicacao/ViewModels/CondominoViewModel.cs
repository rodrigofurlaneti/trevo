using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Entidade;
using Entidade.Uteis;

namespace Aplicacao.ViewModels
{
    public class CondominoViewModel
    {
        public int Id { get; set; }
        public DateTime DataInsercao { get; set; }

        public PessoaViewModel Pessoa { get; set; }
        public string DocumentoCpf => Pessoa?.Documentos?.FirstOrDefault(x => x.Tipo == TipoDocumento.Cpf)?.Numero ?? string.Empty;
        public IList<CondominoVeiculoViewModel> Veiculos { get; set; }
        public UnidadeViewModel Unidade { get; set; }
        public int VagasAdquiridas { get; set; }
        public UnidadeCondomino VagasCondominos { get; set; }
        public string NomeFantasia { get; set; }
        public string RazaoSocial { get; set; }
        public string Cnpj { get; set; }
        public TipoPessoa TipoPessoa { get; set; }

        //Controle de Tela
        public string Cpf { get; set; }

        public CondominoViewModel()
        {
            Pessoa = new PessoaViewModel();
        }

        //public CondominoViewModel(Condomino condomino)
        //{
        //    Id = condomino.Id;
        //    DataInsercao = condomino.Pessoa.DataInsercao;
        //    Unidade = new UnidadeViewModel(condomino?.Unidade ?? new Unidade());
        //    Pessoa = new PessoaViewModel(condomino?.Pessoa ?? new Pessoa());
        //    Veiculos = new CondominoVeiculoViewModel().ListaVeiculos(condomino.Veiculos);
        //    VagasAdquiridas = condomino.VagasAdquiridas;
        //    this.RazaoSocial = condomino?.RazaoSocial;
        //    this.TipoPessoa = condomino.TipoPessoa;
        //    this.Cpf = condomino.Pessoa.DocumentoCpf;
        //    this.Cnpj = condomino.Pessoa.DocumentoCnpj;
        //}

        //public Condomino ToEntity() {
        //    var condomino = new Condomino();

        //    condomino.Id = Id;
        //    condomino.DataInsercao = DateTime.Now;
        //    condomino.Unidade = Unidade?.ToEntity();
        //    condomino.Pessoa = Pessoa?.ToEntity();
        //    condomino.Veiculos = new CondominoVeiculoViewModel().ListaVeiculos(Veiculos);
        //    condomino.VagasAdquiridas = this.VagasAdquiridas;
        //    condomino.RazaoSocial = this.RazaoSocial;
        //    condomino.Pessoa.DocumentoCpf = this.Pessoa.Cpf;
        //    condomino.Pessoa.DocumentoRg = this.Pessoa.Rg;
        //    condomino.Pessoa.DocumentoCnpj = this.Cnpj;
        //    condomino.TipoPessoa = this.TipoPessoa;
        //    return condomino;
        //}

    }
}