using Core.Exceptions;
using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;
using Entidade.Uteis;
using System;

namespace Dominio
{
    public interface INotificacaoAvisoPedidoSeloServico : IBaseServico<NotificacaoAvisoPedidoSelo>
    {
        void Criar(PedidoSelo pedidoSelo, int idUsuario, string msgErro = null, string urlPersonalizada = null);
    }

    public class NotificacaoAvisoPedidoSeloServico : BaseServico<NotificacaoAvisoPedidoSelo, INotificacaoAvisoPedidoSeloRepositorio>, INotificacaoAvisoPedidoSeloServico
    {
        private readonly INotificacaoAvisoPedidoSeloRepositorio _notificacaoAvisoPedidoSeloRepositorio;
        private readonly IUsuarioRepositorio _usuarioRepositorio;
        private readonly ITipoNotificacaoRepositorio _tipoNotificacaoRepositorio;
        private readonly INotificacaoRepositorio _notificacaoRepositorio;

        public NotificacaoAvisoPedidoSeloServico(IUsuarioRepositorio usuarioRepositorio,
                                                 ITipoNotificacaoRepositorio tipoNotificacaoRepositorio,
                                                 INotificacaoAvisoPedidoSeloRepositorio notificacaoAvisoPedidoSeloRepositorio,
                                                 INotificacaoRepositorio notificacaoRepositorio)
        {
            _usuarioRepositorio = usuarioRepositorio;
            _tipoNotificacaoRepositorio = tipoNotificacaoRepositorio;
            _notificacaoAvisoPedidoSeloRepositorio = notificacaoAvisoPedidoSeloRepositorio;
            _notificacaoRepositorio = notificacaoRepositorio;
        }
        
        public void Criar(PedidoSelo pedidoSelo, int idUsuario, string msgErro = null, string urlPersonalizada = null)
        {
            var msgPorStatus = string.Empty;

            if (string.IsNullOrEmpty(msgErro))
                switch (pedidoSelo.StatusPedido)
                {
                    case StatusPedidoSelo.DescontoAprovado:
                        msgPorStatus = $"Desconto do Pedido de Selo [ID: {pedidoSelo.Id}] foi Aprovado no Processo Interno!";
                        break;
                    case StatusPedidoSelo.DescontoReprovado:
                        msgPorStatus = $"Desconto do Pedido de Selo [ID: {pedidoSelo.Id}] foi Reprovado no Processo Interno!";
                        break;
                    case StatusPedidoSelo.AprovadoPeloCliente:
                        msgPorStatus = $"Pedido de Selo [ID: {pedidoSelo.Id}] foi Aprovado pelo Cliente.<br/>Um boleto foi gerado e encaminhado aos contatos da proposta.";
                        break;
                    case StatusPedidoSelo.ReprovadoPeloCliente:
                        msgPorStatus = $"Pedido de Selo [ID: {pedidoSelo.Id}] foi Reprovado pelo Cliente.";
                        break;
                    case StatusPedidoSelo.Cancelado:
                        msgPorStatus = $"Pedido de Selo [ID: {pedidoSelo.Id}] foi Cancelado!";
                        break;
                    default:
                        throw new BusinessRuleException("Status de Pedido não implementado para criação desta notificação!");
                }
            else
                msgPorStatus = $"ERRO: [{msgErro}]";

            var notificacao = new Notificacao
            {
                Usuario = _usuarioRepositorio.GetById(idUsuario),
                TipoNotificacao = _tipoNotificacaoRepositorio.FirstBy(x => x.Entidade == Entidades.PedidoSelo),
                Status = StatusNotificacao.Visualizacao,
                Descricao = msgPorStatus,
                DataInsercao = DateTime.Now,
                DataVencimentoNotificacao = DateTime.Now.AddDays(2),
                AcaoNotificacao = TipoAcaoNotificacao.Aviso,
                UrlPersonalizada = urlPersonalizada
            };

            var notificacaoRetorno = _notificacaoRepositorio.SaveAndReturn(notificacao);
            var notificacaoNova = _notificacaoRepositorio.GetById(notificacaoRetorno);

            var notificacaoAvisoPedidoSelo = new NotificacaoAvisoPedidoSelo
            {
                Notificacao = notificacaoNova,
                PedidoSelo = pedidoSelo
            };

            _notificacaoAvisoPedidoSeloRepositorio.Save(notificacaoAvisoPedidoSelo);
        }
    }
}