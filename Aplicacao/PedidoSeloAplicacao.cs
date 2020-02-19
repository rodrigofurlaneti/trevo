using Aplicacao.Base;
using Aplicacao.ViewModels;
using Core.Extensions;
using Dominio;
using Entidade;
using Entidade.Uteis;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Aplicacao
{
    public interface IPedidoSeloAplicacao : IBaseAplicacao<PedidoSelo>
    {
        void Desbloquear(int id, int idUsuario);
        void Bloquear(int id, int idUsuario);
        void Salvar(PedidoSeloViewModel pedidoSelo, int? idUsuario = null, AcaoWorkflowPedido? acao = null, string descricaoHistorico = null);
        void Salvar(PedidoSelo pedidoSelo, int? idUsuario = null, AcaoWorkflowPedido? acao = null, string descricaoHistorico = null);
        void ClonarPedido(int id, int idUsuario);
        void CancelarPedido(int id, int idUsuario);
        IList<PedidoSelo> ConsultaPedidosPorStatus(int statusid);
        IList<PedidoSeloViewModel> ListaPedidosSelo(int? idPedidoSelo, int? idCliente, int? idConvenio, int? idUnidade, int? idTipoSelo, DateTime? validade, int? idTipoPagamento);
        IList<ClienteViewModel> ListaClientes();
        IList<ConvenioViewModel> ListaConvenios();
        IList<ConvenioViewModel> ListaConvenios(int idUnidade);
        IList<UnidadeViewModel> ListaUnidades(int idConvenio);
        IList<UnidadeViewModel> ListaUnidadesPorCliente(int idCliente);
        IList<PropostaViewModel> ListaPropostas(int idCliente, int idUnidade);
        IList<TipoSelo> ListaTipoSelos();
        IList<TipoSeloViewModel> ListaTipoSelos(int idConvenio, int idUnidade);
        IList<DescontoViewModel> ListaNegociacaoSeloDesconto();
        IList<EmissaoSeloViewModel> ListaEmissaoSelos();
        IList<PedidoSelo> ListaPedidosSelo();
        IList<ChaveValorViewModel> ListaTipoPagamento();
        IList<ChaveValorViewModel> ListaTipoPedidoSelo(StatusPedidoSelo status);
        IList<ChaveValorViewModel> ListaStatusPedido();
        ChaveValorViewModel DescriptografarChave(string chave);
        void GerarBoleto(int idPedido, int idUsuario);
        List<PedidoSeloViewModel> BuscarAprovadosPeloCliente();
        IList<PedidoSeloViewModel> BuscarPedidosSeloAprovados(int? clienteId, int? convenioId, int? unidadeId, int? tipoSeloId, TipoPagamentoSelo? tipoPagamento);
        decimal CalculaValorLancamentoCobranca(PedidoSelo pedido);
        bool ExisteSelosAtivo(int id);
    }

    public class PedidoSeloAplicacao : BaseAplicacao<PedidoSelo, IPedidoSeloServico>, IPedidoSeloAplicacao
    {
        private readonly IClienteAplicacao _clienteAplicacao;
        private readonly IConvenioAplicacao _convenioAplicacao;
        private readonly IUnidadeAplicacao _unidadeAplicacao;
        private readonly IDescontoAplicacao _negociacaoDescontoAplicacao;
        private readonly ITipoSeloAplicacao _tipoSeloAplicacao;
        private readonly IEmissaoSeloAplicacao _emissaoSeloAplicacao;
        private readonly IPropostaAplicacao _propostaSeloAplicacao;
        private readonly IPedidoSeloServico _pedidoSeloServico;
        private readonly ILancamentoCobrancaPedidoSeloAplicacao _lancamentoCobrancaPedidoSeloAplicacao;
        private readonly IGeradorPdfAplicacao _geradorPdfAplicacao;

        public PedidoSeloAplicacao(
            IClienteAplicacao clienteAplicacao,
            IConvenioAplicacao convenioAplicacao,
            IPrecoAplicacao precoAplicacao,
            IUnidadeAplicacao unidadeAplicacao,
            IDescontoAplicacao negociacaoDescontoAplicacao,
            ITipoSeloAplicacao tipoSeloAplicacao,
            IEmissaoSeloAplicacao emissaoSeloAplicacao,
            IPropostaAplicacao propostaSeloAplicacao,
            IUsuarioAplicacao usuarioAplicacao,
            ITipoNotificacaoAplicacao tipoNotificacaoAplicacao,
            IPedidoSeloServico pedidoSeloServico,
            ILancamentoCobrancaPedidoSeloAplicacao lancamentoCobrancaPedidoSeloAplicacao,
            IGeradorPdfAplicacao geradorPdfAplicacao)
        {
            _pedidoSeloServico = pedidoSeloServico;
            _clienteAplicacao = clienteAplicacao;
            _convenioAplicacao = convenioAplicacao;
            _unidadeAplicacao = unidadeAplicacao;
            _negociacaoDescontoAplicacao = negociacaoDescontoAplicacao;
            _tipoSeloAplicacao = tipoSeloAplicacao;
            _emissaoSeloAplicacao = emissaoSeloAplicacao;
            _propostaSeloAplicacao = propostaSeloAplicacao;
            _lancamentoCobrancaPedidoSeloAplicacao = lancamentoCobrancaPedidoSeloAplicacao;
            _geradorPdfAplicacao = geradorPdfAplicacao;
        }

        public void Salvar(PedidoSeloViewModel pedidoSeloViewModel, int? idUsuario = null, AcaoWorkflowPedido? acao = null, string descricaoHistorico = null)
        {
            var pedidoSelo = AutoMapper.Mapper.Map<PedidoSeloViewModel, PedidoSelo>(pedidoSeloViewModel);
            Salvar(pedidoSelo, idUsuario, acao, descricaoHistorico);
        }
        
        public void Salvar(PedidoSelo pedidoSelo, int? idUsuario = null, AcaoWorkflowPedido? acao = null, string descricaoHistorico = null)
        {
            _pedidoSeloServico.Salvar(pedidoSelo, idUsuario, acao, descricaoHistorico);
        }

        public StatusLancamentoCobranca StatusLancamentoCobranca(PedidoSelo pedidoSelo)
        {
            return _lancamentoCobrancaPedidoSeloAplicacao.RetornaStatusPorPedidoSelo(pedidoSelo.Id);
        }

        public void ClonarPedido(int id, int idUsuario)
        {
            var pedido = _pedidoSeloServico.BuscarPorId(id);
            _pedidoSeloServico.ClonarPedido(pedido, idUsuario);
        }

        public void CancelarPedido(int id, int idUsuario)
        {
            var pedido = _pedidoSeloServico.BuscarPorId(id);
            _pedidoSeloServico.CancelarPedido(pedido, idUsuario);
        }

        public bool ExisteSelosAtivo(int id)
        {
            var pedido = _pedidoSeloServico.BuscarPorId(id);
            return _pedidoSeloServico.ExisteSelosAtivo(pedido);
        }

        public IList<PedidoSelo> ConsultaPedidosPorStatus(int statusid)
        {
            return _pedidoSeloServico.ConsultaPedidosPorStatus(statusid);
        }

        public IList<ClienteViewModel> ListaClientes()
        {
            return AutoMapper.Mapper.Map<List<Cliente>, List<ClienteViewModel>>(_clienteAplicacao.Buscar().ToList());
        }

        public IList<ConvenioViewModel> ListaConvenios()
        {
            return AutoMapper.Mapper.Map<List<Convenio>, List<ConvenioViewModel>>(_convenioAplicacao.BuscarPorAtivoComUnidade().ToList());
        }

        public IList<ConvenioViewModel> ListaConvenios(int idUnidade)
        {
            return AutoMapper.Mapper.Map<List<Convenio>, List<ConvenioViewModel>>(_convenioAplicacao.BuscarAtivosPorUnidade(idUnidade).ToList());
        }

        public IList<UnidadeViewModel> ListaUnidades(int idConvenio)
        {
            return AutoMapper.Mapper.Map<List<Unidade>, List<UnidadeViewModel>>(_unidadeAplicacao.BuscarPorConvenio(idConvenio).ToList());
        }

        public IList<UnidadeViewModel> ListaUnidadesPorCliente(int idCliente)
        {
            return AutoMapper.Mapper.Map<List<Unidade>, List<UnidadeViewModel>>(_clienteAplicacao.BuscarPorId(idCliente)?.Unidades?.Select(x => x?.Unidade)?.ToList());
        }

        public IList<DescontoViewModel> ListaNegociacaoSeloDesconto()
        {
            return AutoMapper.Mapper.Map<List<Desconto>, List<DescontoViewModel>>(_negociacaoDescontoAplicacao.Buscar().ToList());
        }

        public IList<TipoSelo> ListaTipoSelos()
        {
            return _tipoSeloAplicacao.Buscar().ToList();
        }

        public IList<TipoSeloViewModel> ListaTipoSelos(int idConvenio, int idUnidade)
        {
            return AutoMapper.Mapper.Map<List<TipoSelo>, List<TipoSeloViewModel>>(_tipoSeloAplicacao.BuscarPorConvenioUnidade(idConvenio, idUnidade).ToList());
        }

        public IList<EmissaoSeloViewModel> ListaEmissaoSelos()
        {
            return _emissaoSeloAplicacao.Buscar().Select(x => new EmissaoSeloViewModel(x)).ToList();
        }

        public IList<PropostaViewModel> ListaPropostas(int idCliente, int idUnidade)
        {
            return _propostaSeloAplicacao
                .BuscarPorClienteUnidade(idCliente, idUnidade)
                .Select(x => new PropostaViewModel(x))
                .ToList();
        }

        public IList<PedidoSelo> ListaPedidosSelo()
        {
            return this.Buscar()?.ToList() ?? new List<PedidoSelo>();
        }

        public IList<ChaveValorViewModel> ListaTipoPagamento()
        {
            return Enum.GetValues(typeof(TipoPagamentoSelo))
                .Cast<TipoPagamentoSelo>()
                .Select(e => new ChaveValorViewModel
                {
                    Id = (int)e,
                    Descricao = e.ToDescription()
                })
                .ToList();
        }

        public IList<ChaveValorViewModel> ListaTipoPedidoSelo(StatusPedidoSelo status)
        {
            return Enum.GetValues(typeof(TipoPedidoSelo))
                .Cast<TipoPedidoSelo>()
                .Where(x => status != StatusPedidoSelo.AprovadoPeloCliente && x == TipoPedidoSelo.Emissao)
                .Select(e => new ChaveValorViewModel
                {
                    Id = (int)e,
                    Descricao = e.ToDescription()
                })
                .ToList();
        }

        public IList<ChaveValorViewModel> ListaStatusPedido()
        {
            return Enum.GetValues(typeof(StatusPedidoSelo))
                .Cast<StatusPedidoSelo>()
                .Select(e => new ChaveValorViewModel
                {
                    Id = (int)e,
                    Descricao = e.ToDescription()
                })
                .ToList();
        }

        public ChaveValorViewModel DescriptografarChave(string chave)
        {
            var dadosDescriptografados = _pedidoSeloServico.DescriptografarChave(chave);
            return new ChaveValorViewModel
            {
                Descricao = dadosDescriptografados[1].ToString(),
                Id = Convert.ToInt32(dadosDescriptografados[0])
            };
        }

        public void GerarBoleto(int idPedido, int idUsuario)
        {
            _pedidoSeloServico.GerarBoleto(idPedido, idUsuario);
        }
        
        public IList<PedidoSeloViewModel> ListaPedidosSelo(int? idPedidoSelo, int? idCliente, int? idConvenio, int? idUnidade, int? idTipoSelo, DateTime? validade, int? idTipoPagamento)
        {
            var listaPedidoSelo = _pedidoSeloServico.ListaPedidosSelo(idPedidoSelo, idCliente, idConvenio, idUnidade, idTipoSelo, validade, idTipoPagamento);
            var listaPedidoSeloViewModel = new List<PedidoSeloViewModel>();

            if (listaPedidoSelo == null || !listaPedidoSelo.Any())
                return listaPedidoSeloViewModel;

            listaPedidoSeloViewModel = listaPedidoSelo.Select(x => new PedidoSeloViewModel(x)).ToList();
            return listaPedidoSeloViewModel;
        }

        public List<PedidoSeloViewModel> BuscarAprovadosPeloCliente()
        {
            var listaPedidoSelo = _pedidoSeloServico.BuscarAprovadosPeloCliente();
            var listaPedidoSeloViewModel = new List<PedidoSeloViewModel>();

            if (listaPedidoSelo == null || !listaPedidoSelo.Any())
                return listaPedidoSeloViewModel;

            listaPedidoSeloViewModel = listaPedidoSelo.Select(x => new PedidoSeloViewModel(x)).ToList();
            return listaPedidoSeloViewModel;
        }

        public void Bloquear(int id, int idUsuario)
        {
            var pedido = _pedidoSeloServico.BuscarPorId(id);
            _pedidoSeloServico.Bloquear(pedido, idUsuario);
        }

        public void Desbloquear(int id, int idUsuario)
        {
            var pedido = _pedidoSeloServico.BuscarPorId(id);
            _pedidoSeloServico.Desbloquear(pedido, idUsuario);
        }

        public IList<PedidoSeloViewModel> BuscarPedidosSeloAprovados(int? clienteId, int? convenioId, int? unidadeId, int? tipoSeloId, TipoPagamentoSelo? tipoPagamento)
        {
            return _pedidoSeloServico.BuscarPedidosSeloAprovados(clienteId, convenioId, unidadeId, tipoSeloId, tipoPagamento)
                                     .Select(x => new PedidoSeloViewModel(x)).ToList();
        }

        public decimal CalculaValorLancamentoCobranca(PedidoSelo pedido)
        {
            return _pedidoSeloServico.CalculaValorLancamentoCobranca(pedido);
        }
    }
}