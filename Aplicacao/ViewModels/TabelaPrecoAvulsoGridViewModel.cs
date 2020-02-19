using Core.Extensions;
using Entidade;
using System;

namespace Aplicacao.ViewModels
{
    public class TabelaPrecoAvulsoGridViewModel
    {
        public int Id { get; set; }
        public int IdUnidade { get; set; }
        public string NomeTabela { get; set; }
        public int TempoToleranciaPagamento { get; set; }
        public int TempoToleranciaDesistencia { get; set; }
        public string Unidade { get; set; }
        public string DataCadastro { get; set; }
        public string Usuario { get; set; }
        public string Status { get; set; }
        public bool ExclusaoGeral { get; set; }

        public TabelaPrecoAvulsoGridViewModel() { }

        public TabelaPrecoAvulsoGridViewModel(TabelaPrecoAvulso entidade, Unidade unidade)
        {
            if (entidade != null && unidade != null)
            {
                Id = entidade.Id;
                IdUnidade = unidade.Id;
                NomeTabela = entidade.Nome;
                TempoToleranciaPagamento = entidade.TempoToleranciaPagamento;
                TempoToleranciaDesistencia = entidade.TempoToleranciaDesistencia;
                Unidade = unidade.Nome;
                DataCadastro = DateTime.Now.ToShortDateString();
                Usuario = entidade.Usuario?.Funcionario?.Pessoa.Nome ?? string.Empty;
                Status = entidade.Status.ToDescription();
                ExclusaoGeral = entidade.ListaUnidade.Count > 1 ? false : true;
            }
        }
    }
}