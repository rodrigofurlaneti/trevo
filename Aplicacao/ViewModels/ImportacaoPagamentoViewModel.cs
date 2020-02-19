using Entidade;
using System;
using System.Collections.Generic;
using System.Data.SqlTypes;

namespace Aplicacao.ViewModels
{
    public class ImportacaoPagamentoViewModel
    {
        public int Id { get; set; }
        public DateTime DataInsercao { get; set; }

        public string Arquivo { get; set; }
        public int Lote { get; set; }
        public DateTime DataPagamento { get; set; }
        public UsuarioViewModel Usuario { get; set; }
        public string Cedente { get; set; }
        public IList<PagamentoViewModel> Pagamento { get; set; }

        public ImportacaoPagamentoViewModel()
        {
            DataInsercao = DateTime.Now;
        }

        public ImportacaoPagamentoViewModel(ImportacaoPagamento importacaoPagamento)
        {
            this.Id = importacaoPagamento?.Id ?? 0;
            this.DataInsercao = importacaoPagamento?.DataInsercao ?? DateTime.Now;
            this.Arquivo = importacaoPagamento?.Arquivo;
            this.Lote = importacaoPagamento?.Lote ?? 0;
            this.DataPagamento = importacaoPagamento?.DataPagamento ?? SqlDateTime.MinValue.Value;
            this.Cedente = importacaoPagamento?.Cedente;
            //this.Usuario = new UsuarioViewModel(importacaoPagamento.Usuario);
            //this.Pagamento = new PagamentoViewModel(importacaoPagamento.Pagamento);
        }

        public ImportacaoPagamento ToEntity() => new ImportacaoPagamento()
        {
            Id = this.Id,
            DataInsercao = this.DataInsercao < System.Data.SqlTypes.SqlDateTime.MinValue.Value ? DateTime.Now : this.DataInsercao,
            Arquivo = Arquivo,
            Lote = Lote,
            DataPagamento = DataPagamento,
            Cedente = Cedente,
            //Usuario = new UsuarioViewModel(importacaoPagamento.Usuario);
            //Pagamento = Pagamento.ToEntity(),
        };
    }
}
