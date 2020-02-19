using Core.Exceptions;
using Core.Extensions;
using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;
using Entidade.Base;
using Entidade.Uteis;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Dominio
{
    public interface INotificacaoServico : IBaseServico<Notificacao>
    {
        void AtualizarStatus(int idNotificacao, Entidades tipoNotificacao, int idUsuario, AcaoNotificacao acao, Stream pdfHtml);
        IList<Notificacao> ObterNotificacoes(int idUsuario, int? tipoNotificacao = null);
        int Informacoes(int idNotificacao, Entidades tipoNotificacao);
        void GerarNotificacaoSeNecessario();
    }

    public class NotificacaoServico : BaseServico<Notificacao, INotificacaoRepositorio>, INotificacaoServico
    {
        private readonly INotificacaoRepositorio _notificacaoRepositorio;
        private readonly IPrecoNotificacaoServico _precoNotificacaoServico;
        private readonly IParametroEquipeNotificacaoServico _parametroEquipeNotificacaoServico;
        private readonly IHorarioUnidadeNotificacaoServico _horarioUnidadeNotificacaoServico;
        private readonly IPedidoSeloNotificacaoServico _pedidoSeloNotificacaoServico;
        private readonly INegociacaoSeloDescontoNotificacaoServico _negociacaoSeloDescontoNotificacaoServico;
        private readonly ITabelaPrecoMensalistaNotificacaoServico _tabelaPrecoMensalistaNotificacaoServico;
        private readonly INotificacaoAvisoPedidoSeloServico _notificacaoAvisoPedidoSeloServico;
        private readonly ITabelaPrecoAvulsoNotificacaoServico _tabelaPrecoAvulsoNotificacaoServico;
        private readonly ITipoNotificacaoRepositorio _tipoNotificacaoRepositorio;
        private readonly IUsuarioRepositorio _usuarioRepositorio;
        private readonly IPedidoLocacaoServico _pedidoLocacaoServico;
        private readonly IMaterialServico _materialServico;
        private readonly INecessidadeCompraServico _necessidadeCompraServico;
        private readonly INecessidadeCompraNotificacaoRepositorio _necessidadeCompraNotificacaoRepositorio;
        private readonly ICotacaoNotificacaoRepositorio _cotacaoNotificacaoRepositorio;
        private readonly IMaterialNotificacaoRepositorio _materialNotificacaoRepositorio;
        private readonly IPedidoLocacaoNotificacaoRepositorio _pedidoLocacaoNotificacaoRepositorio;
        private readonly IPedidoCompraServico _pedidoCompraServico;
        private readonly IOISServico _oisServico;
        private readonly IOrcamentoSinistroServico _orcamentoSinistroServico;
        private readonly IContaPagarServico _contaPagarServico;
        private readonly ILancamentoCobrancaServico _lancamentoCobrancaServico;
        private readonly IRetiradaCofreServico _retiradaCofreServico;
        private readonly IContratoMensalistaServico _contratoMensalistaServico;
        private readonly INotificacaoDesbloqueioReferenciaServico _notificacaoDesbloqueioReferenciaServico;
        private readonly IOcorrenciaServico _ocorrenciaServico;

        public static DateTime DataVerificada { get; set; }

        public NotificacaoServico(
            INotificacaoRepositorio notificacaoRepositorio
            , IPrecoNotificacaoServico precoNotificacaoServico
            , IParametroEquipeNotificacaoServico parametroEquipeNotificacaoServico
            , IHorarioUnidadeNotificacaoServico horarioUnidadeNotificacaoServico
            , IPedidoSeloNotificacaoServico pedidoSeloNotificacaoServico
            , INegociacaoSeloDescontoNotificacaoServico negociacaoSeloDescontoNotificacaoServico
            , ITabelaPrecoMensalistaNotificacaoServico tabelaPrecoMensalistaNotificacaoServico
            , INotificacaoAvisoPedidoSeloServico notificacaoAvisoPedidoSeloServico
            , ITabelaPrecoAvulsoNotificacaoServico tabelaPrecoAvulsoNotificacaoServico
            , ITipoNotificacaoRepositorio tipoNotificacaoRepositorio
            , IUsuarioRepositorio usuarioRepositorio
            , IPedidoLocacaoServico pedidoLocacaoServico
            , IMaterialServico materialServico
            , INecessidadeCompraServico necessidadeCompraServico
            , INecessidadeCompraNotificacaoRepositorio necessidadeCompraNotificacaoRepositorio
            , ICotacaoNotificacaoRepositorio cotacaoNotificacaoRepositorio
            , IMaterialNotificacaoRepositorio materialNotificacaoRepositorio
            , IPedidoLocacaoNotificacaoRepositorio pedidoLocacaoNotificacaoRepositorio
            , IPedidoCompraServico pedidoCompraServico
            , IOISServico oisServico
            , IOrcamentoSinistroServico orcamentoSinistroServico
            , IContaPagarServico contaPagarServico
            , ILancamentoCobrancaServico lancamentoCobrancaServico
            , IRetiradaCofreServico retiradaCofreServico
            , IContratoMensalistaServico contratoMensalistaServico
            , INotificacaoDesbloqueioReferenciaServico notificacaoDesbloqueioReferenciaServico
            , IOcorrenciaServico ocorrenciaServico
            )
        {
            _notificacaoRepositorio = notificacaoRepositorio;
            _precoNotificacaoServico = precoNotificacaoServico;
            _parametroEquipeNotificacaoServico = parametroEquipeNotificacaoServico;
            _horarioUnidadeNotificacaoServico = horarioUnidadeNotificacaoServico;
            _pedidoSeloNotificacaoServico = pedidoSeloNotificacaoServico;
            _negociacaoSeloDescontoNotificacaoServico = negociacaoSeloDescontoNotificacaoServico;
            _tabelaPrecoMensalistaNotificacaoServico = tabelaPrecoMensalistaNotificacaoServico;
            _notificacaoAvisoPedidoSeloServico = notificacaoAvisoPedidoSeloServico;
            _tabelaPrecoAvulsoNotificacaoServico = tabelaPrecoAvulsoNotificacaoServico;
            _tipoNotificacaoRepositorio = tipoNotificacaoRepositorio;
            _usuarioRepositorio = usuarioRepositorio;
            _pedidoLocacaoServico = pedidoLocacaoServico;
            _materialServico = materialServico;
            _necessidadeCompraServico = necessidadeCompraServico;
            _necessidadeCompraNotificacaoRepositorio = necessidadeCompraNotificacaoRepositorio;
            _cotacaoNotificacaoRepositorio = cotacaoNotificacaoRepositorio;
            _materialNotificacaoRepositorio = materialNotificacaoRepositorio;
            _pedidoLocacaoNotificacaoRepositorio = pedidoLocacaoNotificacaoRepositorio;
            _pedidoCompraServico = pedidoCompraServico;
            _oisServico = oisServico;
            _orcamentoSinistroServico = orcamentoSinistroServico;
            _contaPagarServico = contaPagarServico;
            _lancamentoCobrancaServico = lancamentoCobrancaServico;
            _retiradaCofreServico = retiradaCofreServico;
            _contratoMensalistaServico = contratoMensalistaServico;
            _notificacaoDesbloqueioReferenciaServico = notificacaoDesbloqueioReferenciaServico;
            _ocorrenciaServico = ocorrenciaServico;
        }

        public void AtualizarStatus(int idNotificacao, Entidades tipoNotificacao, int idUsuario, AcaoNotificacao acao, Stream pdfHtml)
        {
            var usuario = _usuarioRepositorio.GetById(idUsuario);

            switch (tipoNotificacao)
            {
                case Entidades.DesbloqueioReferencia:
                    _notificacaoDesbloqueioReferenciaServico.AtualizarStatus(idNotificacao, idUsuario, acao);
                    break;
                case Entidades.TabelaPreco:
                    _precoNotificacaoServico.AtualizarStatus(idNotificacao, idUsuario, acao);
                    break;
                case Entidades.HorarioUnidade:
                    _horarioUnidadeNotificacaoServico.AtualizarStatus(idNotificacao, idUsuario, acao);
                    break;
                case Entidades.ParametroEquipe:
                    _parametroEquipeNotificacaoServico.AtualizarStatus(idNotificacao, idUsuario, acao);
                    break;
                case Entidades.Desconto:
                    var retornoDesc = _pedidoLocacaoServico.BuscarPedidoLocacaoNotificacao(idNotificacao);
                    if (retornoDesc != null && retornoDesc.Id > 0)
                        _pedidoLocacaoServico.AtualizarStatus(idNotificacao, usuario, acao);
                    else
                        _negociacaoSeloDescontoNotificacaoServico.AtualizarStatus(idNotificacao, idUsuario, acao);
                    break;
                case Entidades.PedidoSelo:
                    _pedidoSeloNotificacaoServico.AtualizarStatus(idNotificacao, idUsuario, acao, pdfHtml);
                    break;
                case Entidades.TabelaPrecoMensalista:
                    _tabelaPrecoMensalistaNotificacaoServico.AtualizarStatus(idNotificacao, idUsuario, acao);
                    break;
                case Entidades.TabelaPrecoAvulso:
                    _tabelaPrecoAvulsoNotificacaoServico.AtualizarStatus(idNotificacao, idUsuario, acao);
                    break;
                case Entidades.PedidoLocacao:
                    _pedidoLocacaoServico.AtualizarStatus(idNotificacao, usuario, acao);
                    break;
                case Entidades.Material:
                    _materialServico.AtualizarStatus(idNotificacao, usuario, acao);
                    break;
                case Entidades.NecessidadeCompra:
                    _necessidadeCompraServico.AtualizarStatus(idNotificacao, usuario, acao);
                    break;
                case Entidades.Cotacao:
                    _necessidadeCompraServico.AtualizarStatusCotacao(idNotificacao, usuario, acao);
                    break;
                case Entidades.PedidoCompra:
                    _pedidoCompraServico.AtualizarStatus(idNotificacao, usuario, acao);
                    break;
                case Entidades.OrcamentoSinistro:
                    _orcamentoSinistroServico.AtualizarStatus(idNotificacao, usuario, acao);
                    break;
                case Entidades.OrcamentoSinistroCotacao:
                    _orcamentoSinistroServico.AtualizarStatusCotacao(idNotificacao, usuario, acao);
                    break;
                case Entidades.ContasAPagar:
                    _contaPagarServico.AtualizarStatus(idNotificacao, usuario, acao);
                    break;
                default:
                    throw new BusinessRuleException($"Atualização do status da notificação do tipo {tipoNotificacao.ToDescription()} não permitida, favor contate o suporte");
            }
        }

        public int Informacoes(int idNotificacao, Entidades tipoNotificacao)
        {
            var notificacao = Repositorio.GetById(idNotificacao);

            switch (tipoNotificacao)
            {
                case Entidades.DesbloqueioReferencia:
                    var objDesbloqRef = _notificacaoDesbloqueioReferenciaServico.PrimeiroPor(x => x.Notificacao.Id == idNotificacao);
                    return objDesbloqRef.IdRegistro;
                case Entidades.TabelaPreco:
                    var objTabelaPreco = _precoNotificacaoServico.PrimeiroPor(x => x.Notificacao.Id == idNotificacao);
                    return objTabelaPreco.Preco.Id;
                case Entidades.HorarioUnidade:
                    var objHorarioUnidade = _horarioUnidadeNotificacaoServico.PrimeiroPor(x => x.Notificacao.Id == idNotificacao);
                    return objHorarioUnidade.HorarioUnidade.Id;
                case Entidades.ParametroEquipe:
                    var objParametroEquipe = _parametroEquipeNotificacaoServico.PrimeiroPor(x => x.Notificacao.Id == idNotificacao);
                    return objParametroEquipe.ParametroEquipe.Id;
                case Entidades.Desconto:
                    var objNegociacaoDesconto = _negociacaoSeloDescontoNotificacaoServico.PrimeiroPor(x => x.Notificacao.Id == idNotificacao);
                    return objNegociacaoDesconto?.PedidoSelo?.Desconto?.Id ?? 0;
                case Entidades.PedidoSelo:
                    var notificacaoPedido = _pedidoSeloNotificacaoServico.PrimeiroPor(x => x.Notificacao.Id == idNotificacao);
                    if (notificacaoPedido != null)
                        return notificacaoPedido.PedidoSelo.Id;

                    var notificacaoPedidoAviso = _notificacaoAvisoPedidoSeloServico.PrimeiroPor(x => x.Notificacao.Id == idNotificacao);
                    if (notificacaoPedidoAviso != null)
                        return notificacaoPedidoAviso.PedidoSelo.Id;

                    throw new BusinessRuleException("Notificação não encontrada, favor contate o suporte");
                case Entidades.TabelaPrecoMensalista:
                    var objTabelaPrecoMensalista = _tabelaPrecoMensalistaNotificacaoServico.PrimeiroPor(x => x.Notificacao.Id == idNotificacao);
                    return objTabelaPrecoMensalista.TabelaPrecoMensalista.Id;
                case Entidades.TabelaPrecoAvulso:
                    var objTabelaPrecoAvulso = _tabelaPrecoAvulsoNotificacaoServico.PrimeiroPor(x => x.Notificacao.Id == idNotificacao);
                    return objTabelaPrecoAvulso.TabelaPrecoAvulso.Id;
                case Entidades.PedidoLocacao:
                    return _pedidoLocacaoNotificacaoRepositorio.FirstBy(x => x.Notificacao.Id == idNotificacao).PedidoLocacao.Id;
                case Entidades.Material:
                    return _materialNotificacaoRepositorio.FirstBy(x => x.Notificacao.Id == idNotificacao).Material.Id;
                case Entidades.NecessidadeCompra:
                    return _necessidadeCompraNotificacaoRepositorio.FirstBy(x => x.Notificacao.Id == idNotificacao).NecessidadeCompra.Id;
                case Entidades.Cotacao:
                    {
                        var cotacao = _cotacaoNotificacaoRepositorio.FirstBy(x => x.Notificacao.Id == idNotificacao).Cotacao;
                        return _necessidadeCompraServico.PrimeiroPor(x => x.Cotacao.Id == cotacao.Id).Id;
                    }
                case Entidades.PedidoCompra:
                    var pedidoCompraId = _pedidoCompraServico.BuscarIdPelaNotificaoId(idNotificacao);
                    return pedidoCompraId;
                case Entidades.ContasAPagar:
                    return _contaPagarServico.BuscarPelaNotificacaoId(idNotificacao).Id;
                case Entidades.OIS:
                    return _oisServico.BuscarIdPelaNotificacaoId(idNotificacao);
                case Entidades.OrcamentoSinistro:
                    return _orcamentoSinistroServico.BuscarPelaNotificacaoId(idNotificacao).Id;
                case Entidades.OrcamentoSinistroCotacao:
                    {
                        var cotacao = _orcamentoSinistroServico.BuscarCotacaoPelaNotificacaoId(idNotificacao);
                        return _orcamentoSinistroServico.PrimeiroPor(x => x.OrcamentoSinistroCotacao != null && x.OrcamentoSinistroCotacao.Id == cotacao.Id).Id;
                    }
                case Entidades.LancamentoCobranca:
                    return _lancamentoCobrancaServico.BuscarPelaNotificacaoId(idNotificacao).Id;
                case Entidades.RetiradaCofre:
                    return _retiradaCofreServico.BuscarPelaNotificacaoId(idNotificacao)?.Id ?? 0;
                case Entidades.ContratoMensalista:
                    return _contratoMensalistaServico.PrimeiroPor(x => x.ContratoMensalistaNotificacoes.Any(rn => rn.Notificacao.Id == idNotificacao)).Id;
                case Entidades.OcorrenciaAtribuida:
                    {
                        var notificacaoOcorrencia = _notificacaoRepositorio.GetById(idNotificacao);
                        var ocorrenciaId = _ocorrenciaServico.PrimeiroPor(x => x.FuncionarioAtribuido.Pessoa.Id == notificacaoOcorrencia.Usuario.Funcionario.Pessoa.Id).Id;
                        return ocorrenciaId;
                    }
                default:
                    throw new BusinessRuleException($"Informação para notificação do tipo {tipoNotificacao.ToDescription()} não encontrada, favor contate o suporte");
            }
        }

        public IList<Notificacao> ObterNotificacoes(int idUsuario, int? tipoNotificacao = null)
        {
            GerarNotificacaoSeNecessario();
            return _notificacaoRepositorio.ObterNotificacoes(idUsuario, tipoNotificacao);
        }

        public void GerarNotificacaoSeNecessario()
        {
            if (DataVerificada.Date != DateTime.Now.Date)
            {
                _oisServico.GerarNotificacaoSeEmNegociacaoMaisDeMes();
                _orcamentoSinistroServico.GerarNotificacaoSeDataServicoVencida();
                _contratoMensalistaServico.GerarNotificacaoSePertoDeVencer();

                DataVerificada = DateTime.Now;
            }
        }
    }
}