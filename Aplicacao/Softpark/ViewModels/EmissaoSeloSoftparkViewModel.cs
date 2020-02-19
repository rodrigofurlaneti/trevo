using Core.Extensions;
using Entidade;
using Entidade.Uteis;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Aplicacao.ViewModels
{
    /// <summary>
    /// Classe referente a EmissaoSelo
    /// </summary>
    public class EmissaoSeloSoftparkViewModel : BaseSoftparkViewModel
    {
        public DateTime? Validade { get; set; }
        public StatusSelo StatusSelo { get; set; }
        public bool EntregaRealizada { get; set; }
        public DateTime? DataEntrega { get; set; }
        public string Responsavel { get; set; }
        public string ClienteRemetente { get; set; }
        public OperadorSoftparkViewModel UsuarioAlteracaoStatus { get; set; }
        public string NumeroLote { get; set; }
        public PedidoSeloSoftparkViewModel PedidoSelo { get; set; }

        public int EstacionamentoId { get; set; }
        public EstacionamentoSoftparkViewModel Estacionamento { get; set; }

        public EmissaoSeloSoftparkViewModel()
        {
        }

        public EmissaoSeloSoftparkViewModel(EmissaoSelo emissaoSelo)
        {
            Id = emissaoSelo.Id;
            DataInsercao = emissaoSelo.DataInsercao;
            Validade = emissaoSelo.Validade;
            StatusSelo = emissaoSelo.StatusSelo;
            EntregaRealizada = emissaoSelo.EntregaRealizada;
            DataEntrega = emissaoSelo.DataEntrega;
            Responsavel = emissaoSelo.Responsavel;
            ClienteRemetente = emissaoSelo.ClienteRemetente;
            UsuarioAlteracaoStatus = emissaoSelo.UsuarioAlteracaoStatus != null ? new OperadorSoftparkViewModel(emissaoSelo.UsuarioAlteracaoStatus) : null;
            NumeroLote = emissaoSelo.NumeroLote;
            PedidoSelo = emissaoSelo.PedidoSelo != null ? new PedidoSeloSoftparkViewModel(emissaoSelo.PedidoSelo) : null;
            EstacionamentoId = emissaoSelo.PedidoSelo?.Unidade?.Id ?? 0;
            Estacionamento = emissaoSelo.PedidoSelo?.Unidade != null ? new EstacionamentoSoftparkViewModel(emissaoSelo.PedidoSelo.Unidade) : null;
        }
    }
}
