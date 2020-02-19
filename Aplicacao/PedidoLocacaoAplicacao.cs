using Aplicacao.Base;
using Aplicacao.ViewModels;
using AutoMapper;
using Dominio;
using Entidade;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Aplicacao
{
    public interface IPedidoLocacaoAplicacao : IBaseAplicacao<PedidoLocacao>
    {
        void SalvarLocacao(PedidoLocacaoViewModel pedidoLocacaoViewModel, int usuarioId);
        void SalvarPedidoLocacao(PedidoLocacaoViewModel pedidoLocacaoViewModel, int usuarioId);
        IList<PedidoLocacao> ListarPedidoLocacaoFiltro(PedidoLocacaoViewModel filtro);
        bool LiberarControles(int Id);

    }

    public class PedidoLocacaoAplicacao : BaseAplicacao<PedidoLocacao, IPedidoLocacaoServico>, IPedidoLocacaoAplicacao
    {
        private readonly IPedidoLocacaoServico _pedidoLocacaoServico;
        private readonly IDescontoServico _descontoServico;

        public PedidoLocacaoAplicacao(
            IPedidoLocacaoServico pedidoLocacaoServico,
            IDescontoServico descontoServico
            )
        {
            _pedidoLocacaoServico = pedidoLocacaoServico;
            _descontoServico = descontoServico;
        }

        private void Salvar(PedidoLocacaoViewModel pedidoLocacaoViewModel, int usuarioId, string urlPersonalizada = "")
        {
            pedidoLocacaoViewModel.DataInsercao = pedidoLocacaoViewModel.DataInsercao <= System.Data.SqlTypes.SqlDateTime.MinValue.Value ? DateTime.Now : pedidoLocacaoViewModel.DataInsercao;
            var pedidoLocacao = _pedidoLocacaoServico.SalvarComRetorno(Mapper.Map<PedidoLocacao>(pedidoLocacaoViewModel), usuarioId);
            pedidoLocacao.Status = Entidade.Uteis.StatusSolicitacao.Aguardando;

            Notificacao notificacao;
            if (pedidoLocacao.Desconto?.NecessitaAprovacao ?? false)
            {
                var desconto = _descontoServico.BuscarPorId(pedidoLocacao.Desconto.Id);
                desconto.DataVencimentoNotificacao = pedidoLocacao.DataVencimentoNotificacao;
                _descontoServico.Salvar(desconto);

                notificacao = _descontoServico.SalvarNotificacaoComRetorno(desconto, usuarioId, urlPersonalizada);
            }
            else
            {
                notificacao = _pedidoLocacaoServico.SalvarNotificacaoComRetorno(pedidoLocacao, usuarioId, urlPersonalizada);
            }

            _pedidoLocacaoServico.SalvarPedidoLocacaoNotificacao(pedidoLocacao, notificacao);
        }

        public void SalvarLocacao(PedidoLocacaoViewModel pedidoLocacaoViewModel, int usuarioId)
        {
            Salvar(pedidoLocacaoViewModel, usuarioId, $"locacao/index?pedidoId={pedidoLocacaoViewModel.Id}");
        }

        public void SalvarPedidoLocacao(PedidoLocacaoViewModel pedidoLocacaoViewModel, int usuarioId)
        {
            Salvar(pedidoLocacaoViewModel, usuarioId);
        }

        public bool LiberarControles(int Id)
        {
            return _pedidoLocacaoServico.LiberarControles(Id);
        }

        public IList<PedidoLocacao> ListarPedidoLocacaoFiltro(PedidoLocacaoViewModel filtro)
        {
            return _pedidoLocacaoServico.ListarPedidoLocacaoFiltro(filtro?.Unidade?.Id ?? 0,  filtro?.TipoLocacao?.Id ?? 0);
        }


    }
}