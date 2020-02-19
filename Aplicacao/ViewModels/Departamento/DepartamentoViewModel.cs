using System;
using System.Collections.Generic;
using System.Linq;
using Entidade;

namespace Aplicacao.ViewModels
{
    public class DepartamentoViewModel
    {
        public int Id { get; set; }
        public DateTime DataInsercao { get; set; }
        public string Nome { get; set; }
        public string Sigla { get; set; }
        public List<DepartamentoFuncionarioViewModel> DepartamentoResponsaveis { get; set; }

        public string NomeResponsaveis => string.Join("; ", DepartamentoResponsaveis.Select(x => x.Funcionario.Pessoa.Nome));

        //Controle de Tela
        public string Cpf { get; set; }

        public DepartamentoViewModel()
        {
        }

        public DepartamentoViewModel(Departamento departamento)
        {
            Id = departamento.Id;
            DataInsercao = departamento.DataInsercao;
            Nome = departamento.Nome;
            Sigla = departamento.Sigla;
            DepartamentoResponsaveis = departamento.DepartamentoResponsaveis.Select(x => new DepartamentoFuncionarioViewModel(x)).ToList();
        }

        public Departamento ToEntity()
        {
            var departamento = new Departamento();

            departamento.Id = Id;
            departamento.DataInsercao = DateTime.Now;
            departamento.Nome = Nome;
            departamento.Sigla = Sigla;
            departamento.DepartamentoResponsaveis = DepartamentoResponsaveis?.Select(x => x.ToEntity())?.ToList();

            return departamento;
        }
    }
}