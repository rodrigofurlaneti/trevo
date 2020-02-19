using Entidade;
using Entidade.Uteis;
using System;

namespace Aplicacao.ViewModels
{
    public class SeloViewModel
    {
        public int Id { get; set; }
        public int Sequencial { get; set; }
        public Unidade Unidade { get; set; }
        public TipoSelo TipoSelo { get; set; }
        public DateTime? Validade { get; set; }
        public StatusSelo StatusSelo { get; set; }
        public decimal Valor { get; set; }
        public EmissaoSelo EmissaoSelo { get; set; }
        public string NumeroLote { get; set; }
        public string CodigoBarras { get; set; }
        public string UrlImagem { get; set; }

        public SeloViewModel()
        {

        }

        public SeloViewModel(Selo selo)
        {
            if (selo != null)
            {
                Id = selo.Id;
                Sequencial = selo.Sequencial;
                Validade = selo.Validade;
                Valor = selo.Valor;
                EmissaoSelo = selo.EmissaoSelo;
                StatusSelo = selo.EmissaoSelo.StatusSelo;
	            Unidade = selo.EmissaoSelo?.PedidoSelo?.Unidade;
	            NumeroLote = selo.EmissaoSelo?.NumeroLote;
                TipoSelo = selo.EmissaoSelo?.PedidoSelo?.TipoSelo;
            }
        }

        public Selo ToEntity()
        {
            var entidade = new Selo
            {
                Id = Id,
                Sequencial = Sequencial,
                Validade = Validade,
                Valor = Valor,
                EmissaoSelo = EmissaoSelo
            };

            return entidade;
        }
    }
}