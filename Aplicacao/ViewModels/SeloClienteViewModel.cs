
using Entidade;
using Entidade.Uteis;
using System;

namespace Aplicacao.ViewModels
{
    public class SeloClienteViewModel
    {
        public int Id { get; set; }        
        public DateTime DataInsercao { get; set; }
        public ClienteViewModel Cliente { get; set; }
        public TipoPagamentoSelo TipoPagamentoSelo { get; set; }
        public int ValidadeSelo { get; set; }
        public int PrazoPagamentoSelo { get; set; }
        public bool EmissaoNF { get; set; }

        public SeloClienteViewModel()
        {
        }

        public SeloClienteViewModel(SeloCliente seloCliente)
        {
            Id = seloCliente?.Id ?? 0;
            DataInsercao = seloCliente.DataInsercao;

            Cliente = seloCliente.Cliente == null ? new ClienteViewModel() : new ClienteViewModel() { Id = seloCliente.Cliente.Id};
            TipoPagamentoSelo = seloCliente.TipoPagamentoSelo;
            ValidadeSelo = seloCliente.ValidadeSelo;
            PrazoPagamentoSelo = seloCliente.PrazoPagamentoSelo;
            EmissaoNF = seloCliente.EmissaoNF;
        }

        public SeloCliente ToEntity()
        {
            return new SeloCliente
            {
                Id = Id,
                DataInsercao = DataInsercao != DateTime.MinValue ? DataInsercao : DateTime.Now,
                Cliente = Cliente == null ? new Cliente() : Cliente.ToEntity(),
                TipoPagamentoSelo = TipoPagamentoSelo,
                ValidadeSelo = ValidadeSelo,                
                PrazoPagamentoSelo = PrazoPagamentoSelo,
                EmissaoNF = EmissaoNF
            };
        }
    }
}
