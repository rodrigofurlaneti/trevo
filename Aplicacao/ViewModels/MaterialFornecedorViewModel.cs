using Entidade;
using System;

namespace Aplicacao.ViewModels
{
    public class MaterialFornecedorViewModel
    {
        public int Id { get; set; }
        public DateTime DataInsercao { get; set; }
        public MaterialViewModel Material { get; set; }
        public FornecedorViewModel Fornecedor { get; set; }
        public bool EhPersonalizado { get; set; }
        public int QuantidadeParaPedidoAutomatico { get; set; }
    }
}