using Entidade.Uteis;
using System;
using System.Collections.Generic;

namespace Aplicacao.ViewModels
{
    public class CotacaoViewModel
    {
        public int Id { get; set; }
        public DateTime DataInsercao { get; set; }
        public List<CotacaoMaterialFornecedorViewModel> MaterialFornecedores { get; set; }
        public StatusCotacao Status { get; set; }
    }
}