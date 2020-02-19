using Entidade;
using Entidade.Uteis;
using System;

namespace Aplicacao.ViewModels
{
    public class ContasAPagarItemViewModel
    {
        public int Id { get; set; }
        public DateTime DataInsercao { get; set; }
        public ContaContabilViewModel ContaContabil { get; set; }
        public UnidadeViewModel Unidade { get; set; }
        public decimal Valor { get; set; }

        public ContasAPagarItemViewModel()
        {
            DataInsercao = DateTime.Now;
        }

        public ContasAPagarItemViewModel(ContaContabil contaContabil, UnidadeViewModel unidade, decimal valor)
        {
            DataInsercao = DateTime.Now;
            ContaContabil = new ContaContabilViewModel(contaContabil);// { Id = contaContabilId };
            Unidade = unidade;
            Valor = valor;
        }
    }
}