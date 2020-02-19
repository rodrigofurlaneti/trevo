using Core.Exceptions;
using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;
using Entidade.Uteis;

namespace Dominio
{
    public interface INegociacaoSeloDescontoNotificacaoServico : IBaseServico<NegociacaoSeloDescontoNotificacao>
    {
        void AtualizarStatus(int idNotificacao, int idUsuario, AcaoNotificacao acao);
        void Criar(PedidoSelo pedidoSelo, int idUsuario);
    }

    public class NegociacaoSeloDescontoNotificacaoServico : BaseServico<NegociacaoSeloDescontoNotificacao, INegociacaoSeloDescontoNotificacaoRepositorio>, INegociacaoSeloDescontoNotificacaoServico
    {
        private readonly INegociacaoSeloDescontoNotificacaoRepositorio _negociacaoSeloDescontoNotificacaoRepositorio;
        private readonly IUsuarioRepositorio _usuarioRepositorio;
        private readonly ITipoNotificacaoRepositorio _tipoNotificacaoRepositorio;
        private readonly INotificacaoRepositorio _notificacaoRepositorio;
        private readonly IPedidoSeloNotificacaoServico _pedidoSeloNotificacaoServico;
        private readonly IPedidoSeloServico _pedidoSeloServico;

        public NegociacaoSeloDescontoNotificacaoServico(
            IUsuarioRepositorio usuarioRepositorio,
            ITipoNotificacaoRepositorio tipoNotificacaoRepositorio,
            INegociacaoSeloDescontoNotificacaoRepositorio negociacaoSeloDescontoNotificacaoRepositorio,
            IDescontoRepositorio negociacaoSeloDescontoRepositorio,
            INotificacaoRepositorio notificacaoRepositorio,
            IPedidoSeloNotificacaoServico pedidoSeloNotificacaoServico,
            IPedidoSeloServico pedidoSeloServico)
        {
            _usuarioRepositorio = usuarioRepositorio;
            _tipoNotificacaoRepositorio = tipoNotificacaoRepositorio;
            _negociacaoSeloDescontoNotificacaoRepositorio = negociacaoSeloDescontoNotificacaoRepositorio;
            _notificacaoRepositorio = notificacaoRepositorio;
            _pedidoSeloNotificacaoServico = pedidoSeloNotificacaoServico;
            _pedidoSeloServico = pedidoSeloServico;
        }

        public void AtualizarStatus(int idNotificacao, int idUsuario, AcaoNotificacao acao)
        {
            switch (acao)
            {
                case AcaoNotificacao.Aprovado:
                    Aprovar(idNotificacao, idUsuario);
                    break;
                case AcaoNotificacao.Reprovado:
                    Reprovar(idNotificacao, idUsuario);
                    break;
            }
        }

        public void Aprovar(int idNotificacao, int idUsuario)
        {
            var negociacaoSeloDescontoNotificacao = _negociacaoSeloDescontoNotificacaoRepositorio.FirstBy(x => x.Notificacao.Id == idNotificacao);

            if (negociacaoSeloDescontoNotificacao.Notificacao.Status == StatusNotificacao.Reprovado)
            {
                throw new BusinessRuleException("A notificação encontra-se 'Reprovada'. Não é possível 'Aprovar'!");
            }

            negociacaoSeloDescontoNotificacao.Notificacao.Status = StatusNotificacao.Aprovado;
            _negociacaoSeloDescontoNotificacaoRepositorio.Save(negociacaoSeloDescontoNotificacao);
            var pedidoSelo = _pedidoSeloServico.BuscarPorId(negociacaoSeloDescontoNotificacao.PedidoSelo.Id);
            _pedidoSeloServico.Salvar(pedidoSelo, idUsuario, AcaoWorkflowPedido.Aprovar);
        }

        public void Reprovar(int idNotificacao, int idUsuario)
        {
            var negociacaoSeloDescontoNotificacao = _negociacaoSeloDescontoNotificacaoRepositorio.FirstBy(x => x.Notificacao.Id == idNotificacao);

            if (negociacaoSeloDescontoNotificacao.Notificacao.Status == StatusNotificacao.Aprovado)
            {
                throw new BusinessRuleException("A notificação encontra-se 'Aprovada'. Não é possível 'Reprovar'!");
            }

            negociacaoSeloDescontoNotificacao.Notificacao.Status = StatusNotificacao.Reprovado;
            _negociacaoSeloDescontoNotificacaoRepositorio.Save(negociacaoSeloDescontoNotificacao);
            var pedidoSelo = _pedidoSeloServico.BuscarPorId(negociacaoSeloDescontoNotificacao.PedidoSelo.Id);
            _pedidoSeloServico.Salvar(pedidoSelo, idUsuario, AcaoWorkflowPedido.Reprovar);
        }

        public void Criar(PedidoSelo pedidoSelo, int idUsuario)
        {
            _negociacaoSeloDescontoNotificacaoRepositorio.Criar(pedidoSelo, idUsuario);
        }
    }
}