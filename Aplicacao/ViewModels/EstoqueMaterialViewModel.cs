using System;

namespace Aplicacao.ViewModels
{
    public class EstoqueMaterialViewModel
    {
        public int Id { get; set; }
        public DateTime DataInsercao { get; set; }
        public MaterialViewModel Material { get; set; }
        public EstoqueViewModel Estoque { get; set; }
        public int Quantidade { get; set; }
        public string Preco { get; set; }
        public string PrimeiroPreco { get; set; }
        public string ValorTotal { get; set; }
    }
}
