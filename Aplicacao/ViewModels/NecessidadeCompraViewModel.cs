using Entidade.Uteis;
using System;
using System.Collections.Generic;

namespace Aplicacao.ViewModels
{
    public class NecessidadeCompraViewModel
    {
        public int Id { get; set; }
        public DateTime DataInsercao { get; set; }

        public DateTime DataNotificacaoValidade { get; set; }

        public List<NecessidadeCompraMaterialFornecedorViewModel> MaterialFornecedores { get; set; }

        public CotacaoViewModel Cotacao { get; set; }

        public StatusNecessidadeCompra StatusNecessidadeCompra { get; set; }
    }
}