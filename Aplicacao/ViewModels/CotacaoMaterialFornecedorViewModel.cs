using System;

namespace Aplicacao.ViewModels
{
    public class CotacaoMaterialFornecedorViewModel
    {
        public int Id { get; set; }
        public DateTime DataInsercao { get; set; }

        public CotacaoViewModel Cotacao { get; set; }
        public MaterialViewModel Material { get; set; }
        public FornecedorViewModel Fornecedor { get; set; }
        public int Quantidade { get; set; }
        public string Valor { get; set; }
        public string ValorTotal { get; set; }
    }
}