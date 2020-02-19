using Entidade;
using System;

namespace Aplicacao.ViewModels
{
    public class ContaContabilViewModel
    {

        public int Id { get; set; }
        public string Descricao { get; set; }
        public bool Ativo { get; set; }
        public bool Fixa { get; set; }
        public DateTime DataInsercao { get; set; }
        public int Hierarquia { get; set; }

        public string DescricaoCompleta
        {
            get { return Hierarquia + " - " + Descricao; }
        }

        public ContaContabilViewModel()
        {
            DataInsercao = DateTime.Now;
        }

        public ContaContabilViewModel(ContaContabil ContaContabil)
        {
            Id = ContaContabil.Id;
            Descricao = ContaContabil.Descricao;
            Ativo = ContaContabil.Ativo;
            Fixa = ContaContabil.Fixa;
            DataInsercao = ContaContabil.DataInsercao;
            Hierarquia = ContaContabil.Hierarquia;
        }

        public ContaContabil ToEntity()
        {
            var entidade = new ContaContabil
            {
                Id = Id,
                Descricao = Descricao,
                Ativo = Ativo,
                Fixa = Fixa,
                DataInsercao = DataInsercao < System.Data.SqlTypes.SqlDateTime.MinValue.Value ? DateTime.Now : DataInsercao,
                Hierarquia = Hierarquia
            };

            return entidade;
        }
    }
}
