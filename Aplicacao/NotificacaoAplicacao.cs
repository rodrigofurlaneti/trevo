using Aplicacao.Base;
using Aplicacao.ViewModels;
using Dominio;
using Entidade;
using Entidade.Uteis;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web.Mvc;

namespace Aplicacao
{
    public interface INotificacaoAplicacao : IBaseAplicacao<Notificacao>
    {
        void AtualizarStatus(int idNotificacao, Entidades tipoNotificacao, int idUsuario, AcaoNotificacao acao, ControllerContext controllerContext);
        IList<NotificacaoViewModel> ObterNotificacoes(int idUsuario, int? tipoNotificacao = null);
        int Informacoes(int idNotificacao, Entidades tipoNotificacao);
    }

    public class NotificacaoAplicacao : BaseAplicacao<Notificacao, INotificacaoServico>, INotificacaoAplicacao
    {
        private readonly INotificacaoServico _notificacaoServico;
        private readonly IPropostaAplicacao _propostaAplicacao;
        private readonly IPedidoSeloNotificacaoServico _pedidoSeloNotificacaoServico;

        public NotificacaoAplicacao(
            INotificacaoServico notificacaoServico,
            IPropostaAplicacao propostaAplicacao,
            IPedidoSeloNotificacaoServico pedidoSeloNotificacaoServico)
        {
            _notificacaoServico = notificacaoServico;
            _propostaAplicacao = propostaAplicacao;
            _pedidoSeloNotificacaoServico = pedidoSeloNotificacaoServico;
        }

        public void AtualizarStatus(int idNotificacao, Entidades tipoNotificacao, int idUsuario, AcaoNotificacao acao, ControllerContext controllerContext)
        {
            Stream pdfHtml = null;
            if (Entidades.PedidoSelo == tipoNotificacao)
            {
                var pedidoSelo = _pedidoSeloNotificacaoServico.BuscarPorIdNotificacao(idNotificacao);
                if (pedidoSelo != null && pedidoSelo.StatusPedido != StatusPedidoSelo.AprovadoPeloCliente)
                    pdfHtml = _propostaAplicacao.GerarPdfStream(pedidoSelo, controllerContext);
            }

            _notificacaoServico.AtualizarStatus(idNotificacao, tipoNotificacao, idUsuario, acao, pdfHtml);
        }

        public int Informacoes(int idNotificacao, Entidades tipoNotificacao)
        {
            return _notificacaoServico.Informacoes(idNotificacao, tipoNotificacao);
        }

        public IList<NotificacaoViewModel> ObterNotificacoes(int idUsuario, int? tipoNotificacao = null)
        {
            return _notificacaoServico.ObterNotificacoes(idUsuario, tipoNotificacao)?.Select(x => new NotificacaoViewModel(x))?.ToList() ?? new List<NotificacaoViewModel>();
        }
    }
}