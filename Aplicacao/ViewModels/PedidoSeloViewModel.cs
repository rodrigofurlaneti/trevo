using Core.Extensions;
using Entidade;
using Entidade.Uteis;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Aplicacao.ViewModels
{
    public class PedidoSeloViewModel
    {
        public int Id { get; set; }
        public ClienteViewModel Cliente { get; set; }
        public ConvenioViewModel Convenio { get; set; }
        public DateTime DataInsercao { get; set; }
        public UnidadeViewModel Unidade { get; set; }
        public TipoPagamentoSelo TiposPagamento { get; set; }
        public DescontoViewModel Desconto { get; set; }
        public StatusPedidoSelo StatusPedido { get; set; }
        public DateTime ValidadePedido { get; set; }
        public TipoSeloViewModel TipoSelo { get; set; }
        public int Quantidade { get; set; }
        public int DiasVencimento { get; set; }
        public DateTime DataVencimento { get; set; }
        public TipoPedidoSelo TipoPedidoSelo { get; set; }
        public int EmissaoSelo { get; set; }
        public string NumeroLoteSelo { get; set; }
        public StatusSelo? StatusEmissaoSelo { get; set; }
        public PropostaViewModel Proposta { get; set; }
        public UsuarioViewModel Usuario { get; set; }
        public IList<PedidoSeloNotificacaoViewModel> Notificacoes { get; set; }
        public StatusLancamentoCobranca StatusLancamentoCobranca { get; set; }
        public bool SeloComValidade => TipoSelo?.ComValidade ?? false;
        public string DescricaoStatusPedido => StatusPedido.ToDescription();
        public string DescricaoTipoPedido => TipoPedidoSelo.ToDescription();

        public bool TemBoleto { get; set; }

        public PedidoSeloViewModel()
        {
            DataInsercao = DateTime.Now;
            TipoPedidoSelo = TipoPedidoSelo.Emissao;
        }

        public PedidoSeloViewModel(PedidoSelo pedido)
        {
            if (pedido != null)
            {
                Id = pedido.Id;
                Cliente = new ClienteViewModel(pedido?.Cliente);
                Convenio = new ConvenioViewModel(pedido?.Convenio);
                Unidade = new UnidadeViewModel(pedido?.Unidade);
                DataInsercao = pedido.DataInsercao;
                TiposPagamento = pedido.TiposPagamento;
                Desconto = new DescontoViewModel(pedido?.Desconto);
                ValidadePedido = pedido.ValidadePedido;
                TipoSelo = new TipoSeloViewModel(pedido?.TipoSelo);
                Quantidade = pedido.Quantidade;
                DiasVencimento = pedido.DiasVencimento;
                DataVencimento = pedido.DataVencimento;
                TipoPedidoSelo = pedido.TipoPedidoSelo;
                EmissaoSelo = pedido?.EmissaoSelo?.Id ?? 0;
                NumeroLoteSelo = pedido?.EmissaoSelo?.NumeroLote ?? string.Empty;
                StatusEmissaoSelo = pedido?.EmissaoSelo?.StatusSelo;
                Proposta = new PropostaViewModel(pedido?.Proposta);
                Notificacoes = pedido?.Notificacoes?.Select(x => new PedidoSeloNotificacaoViewModel()).ToList();
                StatusPedido = pedido.StatusPedido;
                StatusLancamentoCobranca = pedido?.UltimoLancamento?.StatusLancamentoCobranca ?? StatusLancamentoCobranca.EmAberto;

                if (pedido?.Usuario != null)
                    Usuario = new UsuarioViewModel(pedido.Usuario);
            }
        }

        public PedidoSelo ToEntity()
        {
            return new PedidoSelo
            {
                Cliente = Cliente?.ToEntity(),
                Convenio = Convenio?.ToEntity(),
                DataInsercao = DataInsercao,
                Desconto = Desconto?.ToEntity(),
                DiasVencimento = DiasVencimento,
                DataVencimento = DataVencimento,
                //EmissaoSelo
                Id = Id,
                //LancamentoCobranca
                //Notificacoes
                //PedidoSeloEmails
                //PedidoSeloHistorico
                Proposta = Proposta?.ToEntity(),
                Quantidade = Quantidade,
                TipoPedidoSelo = TipoPedidoSelo,
                TipoSelo = TipoSelo?.ToEntity(),
                TiposPagamento = TiposPagamento,
                Unidade = Unidade?.ToEntity(),
                Usuario = Usuario?.ToEntity(),
                ValidadePedido = ValidadePedido
            };
        }
    }
}