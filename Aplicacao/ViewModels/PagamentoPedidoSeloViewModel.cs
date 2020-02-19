using Entidade;
using Entidade.Uteis;
using System;
using System.Data.SqlTypes;

namespace Aplicacao.ViewModels
{
    public class PagamentoPedidoSeloViewModel
    {
        public Cliente Cliente { get; set; }
        public Convenio Convenio { get; set; }
        public Unidade Unidade { get; set; }
        public TipoPagamentoSelo TiposPagamento { get; set; }
        public TipoSelo TipoSelo { get; set; }

        public PagamentoPedidoSeloViewModel()
        {
        }
    }
}
