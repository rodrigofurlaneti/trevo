using System;
using System.Collections.Generic;

namespace Aplicacao.ViewModels
{
    public class MaterialViewModel
    {
        public int Id { get; set; }
        public DateTime DataInsercao { get; set; }

        public string Nome { get; set; }
        public TipoMaterialViewModel TipoMaterial { get; set; }
        public string Descricao { get; set; }
        public string Altura { get; set; }
        public string Imagem { get; set; }
        public string Largura { get; set; }
        public string Profundidade { get; set; }
        public string Comprimento { get; set; }
        public string EAN { get; set; }
        public bool EhUmAtivo { get; set; }
        public int EstoqueMaximo { get; set; }
        public int EstoqueMinimo { get; set; }
        public int QuantidadeTotalEstoque { get; set; }
        public List<MaterialFornecedorViewModel> MaterialFornecedores { get; set; }
    }
}
