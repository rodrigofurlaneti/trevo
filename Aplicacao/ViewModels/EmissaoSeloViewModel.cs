using Entidade;
using Entidade.Uteis;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Aplicacao.ViewModels
{
    public class EmissaoSeloViewModel
    {
        public int Id { get; set; }

        public DateTime DataInsercao { get; set; }

        public DateTime? Validade { get; set; }
        public StatusSelo StatusSelo { get; set; }
        public bool EntregaRealizada { get; set; }
        public DateTime? DataEntrega { get; set; }
        public string Responsavel { get; set; }
        public string ClienteRemetente { get; set; }
        public IList<SeloViewModel> Selo { get; set; }
        public PedidoSeloViewModel PedidoSelo { get; set; }
        public string NomeImpressaoSelo { get; set; }

        public EmissaoSeloViewModel()
        {

        }

        public EmissaoSeloViewModel(EmissaoSelo emissaoSelo)
        {
            if (emissaoSelo != null)
            {
                Id = emissaoSelo.Id;
                DataInsercao = emissaoSelo.DataInsercao;
                Validade = emissaoSelo.Validade;
                StatusSelo = emissaoSelo.StatusSelo;
                ClienteRemetente = emissaoSelo.ClienteRemetente;
                EntregaRealizada = emissaoSelo.EntregaRealizada;
                DataEntrega = emissaoSelo.DataEntrega;
                Responsavel = emissaoSelo.Responsavel;
                PedidoSelo = emissaoSelo?.PedidoSelo != null ? new PedidoSeloViewModel(emissaoSelo?.PedidoSelo) : null;
                Selo = emissaoSelo.Selo?.Select(x => new SeloViewModel(x)).ToList();
                NomeImpressaoSelo = emissaoSelo.NomeImpressaoSelo;
            }
        }

        public EmissaoSelo ToEntity()
        {
            var entidade = new EmissaoSelo
            {
                Id = Id,
                DataInsercao = DataInsercao,
                Validade = Validade,
                StatusSelo = StatusSelo,
                ClienteRemetente = ClienteRemetente,
                EntregaRealizada = EntregaRealizada,
                DataEntrega = DataEntrega,
                PedidoSelo = PedidoSelo?.ToEntity() ?? new PedidoSelo(),
                Responsavel = Responsavel,
                Selo = Selo?.Select(x => x.ToEntity())?.ToList() ?? new List<Selo>(),
                NomeImpressaoSelo = NomeImpressaoSelo
            };

            return entidade;
        }
    }
}