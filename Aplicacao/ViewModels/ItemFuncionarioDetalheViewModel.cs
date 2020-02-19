using System;

namespace Aplicacao.ViewModels
{
    public class ItemFuncionarioDetalheViewModel
    {
        public ItemFuncionarioDetalheViewModel(MaterialViewModel material, EstoqueMaterialViewModel estoqueMaterial, string valor, int quantidade, string valorTotal)
        {
            DataInsercao = DateTime.Now;
            Material = material;
            EstoqueMaterial = estoqueMaterial;
            Valor = valor;
            Quantidade = quantidade;
            ValorTotal = valorTotal;
        }

        public int Id { get; set; }
        public DateTime DataInsercao { get; set; }
        public MaterialViewModel Material { get; set; }
        public EstoqueMaterialViewModel EstoqueMaterial { get; set; }
        public string Valor { get; set; }
        public int Quantidade { get; set; }
        public string ValorTotal { get; set; }
    }
}