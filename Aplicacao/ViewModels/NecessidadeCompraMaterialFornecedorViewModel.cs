using System;

namespace Aplicacao.ViewModels
{
    public class NecessidadeCompraMaterialFornecedorViewModel
    {
        public int Id { get; set; }
        public DateTime DataInsercao { get; set; }

        public NecessidadeCompraViewModel NecessidadeCompra { get; set; }
        public MaterialViewModel Material { get; set; }
        public FornecedorViewModel Fornecedor { get; set; }

        public int Quantidade { get; set; }
    }
}