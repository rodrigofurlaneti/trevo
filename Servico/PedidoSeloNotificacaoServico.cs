using Core.Exceptions;
using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;
using Entidade.Uteis;
using System.IO;

namespace Dominio
{
    public interface IPedidoSeloNotificacaoServico : IBaseServico<PedidoSeloNotificacao>
    {
        void AtualizarStatus(int idNotificacao, int idUsuario, AcaoNotificacao acao, Stream pdfHtml);
        void Criar(PedidoSelo PedidoSelo, int idUsuario);
        void CriarNotificacaoBloqueio(PedidoSelo PedidoSelo, int idUsuario);
        PedidoSelo BuscarPorIdNotificacao(int idNotificacao);
    }

    public class PedidoSeloNotificacaoServico : BaseServico<PedidoSeloNotificacao, IPedidoSeloNotificacaoRepositorio>, IPedidoSeloNotificacaoServico
    {
        private readonly IPedidoSeloNotificacaoRepositorio _pedidoSeloNotificacaoRepositorio;
        private readonly IUsuarioRepositorio _usuarioRepositorio;
        private readonly ITipoNotificacaoRepositorio _tipoNotificacaoRepositorio;
        private readonly INotificacaoRepositorio _notificacaoRepositorio;
        private readonly IPedidoSeloServico _pedidoSeloServico;

        public PedidoSeloNotificacaoServico(
            IUsuarioRepositorio usuarioRepositorio,
            ITipoNotificacaoRepositorio tipoNotificacaoRepositorio,
            IPedidoSeloNotificacaoRepositorio pedidoSeloNotificacaoRepositorio,
            IPedidoSeloRepositorio pedidoSeloRepositorio,
            INotificacaoRepositorio notificacaoRepositorio,
            IPedidoSeloServico pedidoSeloServico)
        {
            _usuarioRepositorio = usuarioRepositorio;
            _tipoNotificacaoRepositorio = tipoNotificacaoRepositorio;
            _pedidoSeloNotificacaoRepositorio = pedidoSeloNotificacaoRepositorio;
            _notificacaoRepositorio = notificacaoRepositorio;
            _pedidoSeloServico = pedidoSeloServico;
        }

        public void AtualizarStatus(int idNotificacao, int idUsuario, AcaoNotificacao acao, Stream pdfHtml)
        {
            switch (acao)
            {
                case AcaoNotificacao.Aprovado:
                    Aprovar(idNotificacao, idUsuario, pdfHtml);
                    break;
                case AcaoNotificacao.Reprovado:
                    Reprovar(idNotificacao, idUsuario);
                    break;
            }
        }

        public void Aprovar(int idNotificacao, int idUsuario, Stream pdfHtml)
        {
            var pedidoSeloNotificacao = _pedidoSeloNotificacaoRepositorio.FirstBy(x => x.Notificacao.Id == idNotificacao);

            if (pedidoSeloNotificacao.Notificacao.Status == StatusNotificacao.Reprovado)
            {
                throw new BusinessRuleException("A notificação encontra-se 'Reprovada'. Não é possível 'Aprovar'!");
            }

            var pedido = _pedidoSeloServico.BuscarPorId(pedidoSeloNotificacao.PedidoSelo.Id);
            _pedidoSeloServico.Salvar(pedido, idUsuario, AcaoWorkflowPedido.Aprovar);
            var notificacao = _notificacaoRepositorio.GetById(pedidoSeloNotificacao.Notificacao.Id);
            notificacao.Aprovador = _usuarioRepositorio.GetById(idUsuario);
            notificacao.Status = StatusNotificacao.Aprovado;
            _notificacaoRepositorio.Save(notificacao);

            pedido = _pedidoSeloServico.BuscarPorId(pedido.Id);
            if (pdfHtml != null)
                _pedidoSeloServico.EnviaEmailPropostaAprovada(pedido, idUsuario, pdfHtml);
        }

        public void Reprovar(int idNotificacao, int idUsuario)
        {
            var pedidoSeloNotificacao = _pedidoSeloNotificacaoRepositorio.FirstBy(x => x.Notificacao.Id == idNotificacao);

            if (pedidoSeloNotificacao.Notificacao.Status == StatusNotificacao.Aprovado)
            {
                throw new BusinessRuleException("A notificação encontra-se 'Aprovada'. Não é possível 'Reprovar'!");
            }

            var pedido = _pedidoSeloServico.BuscarPorId(pedidoSeloNotificacao.PedidoSelo.Id);
            _pedidoSeloServico.Salvar(pedido, idUsuario, AcaoWorkflowPedido.Reprovar);
            pedidoSeloNotificacao.PedidoSelo = pedido;
            _pedidoSeloNotificacaoRepositorio.Save(pedidoSeloNotificacao);
            var notificacao = _notificacaoRepositorio.GetById(pedidoSeloNotificacao.Notificacao.Id);
            notificacao.Status = StatusNotificacao.Reprovado;
            _notificacaoRepositorio.Save(notificacao);
        }

        public void CriarNotificacaoBloqueio(PedidoSelo pedidoSelo, int idUsuario)
        {
            _pedidoSeloNotificacaoRepositorio.CriarNotificacaoBloqueio(pedidoSelo, idUsuario);
        }

        public void Criar(PedidoSelo pedidoSelo, int idUsuario)
        {
            _pedidoSeloNotificacaoRepositorio.Criar(pedidoSelo, idUsuario);
        }

        public PedidoSelo BuscarPorIdNotificacao(int idNotificacao)
        {
            return _pedidoSeloNotificacaoRepositorio.BuscarPorIdNotificacao(idNotificacao);
        }
    }
}