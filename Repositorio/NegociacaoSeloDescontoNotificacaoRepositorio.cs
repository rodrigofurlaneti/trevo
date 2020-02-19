using Dominio.IRepositorio;
using Entidade;
using Entidade.Uteis;
using Repositorio.Base;
using System;

namespace Repositorio
{
    public class NegociacaoSeloDescontoNotificacaoRepositorio : NHibRepository<NegociacaoSeloDescontoNotificacao>, INegociacaoSeloDescontoNotificacaoRepositorio
    {
        private readonly ITipoNotificacaoRepositorio _tipoNotificacaoRepositorio;
        private readonly INotificacaoRepositorio _notificacaoRepositorio;

        public NegociacaoSeloDescontoNotificacaoRepositorio(
            NHibContext context,
            ITipoNotificacaoRepositorio tipoNotificacaoRepositorio,
            INotificacaoRepositorio notificacaoRepositorio)
            : base(context)
        {
            _tipoNotificacaoRepositorio = tipoNotificacaoRepositorio;
            _notificacaoRepositorio = notificacaoRepositorio;
        }

        public void Criar(PedidoSelo pedido, int idUsuario)
        {
            var notificacao = new Notificacao
            {
                Usuario = new Usuario { Id = idUsuario },
                TipoNotificacao = _tipoNotificacaoRepositorio.FirstBy(x => x.Entidade == Entidades.Desconto),
                Status = StatusNotificacao.Aguardando,
                Descricao = $"Pedido Selo (aprov. desc.) ID: {pedido.Id.ToString()}",
                DataInsercao = DateTime.Now,
                AcaoNotificacao = TipoAcaoNotificacao.AprovarReprovar,
                DataVencimentoNotificacao = pedido.ValidadePedido
            };
            var notificacaoRetorno = _notificacaoRepositorio.SaveAndReturn(notificacao);
            var notificacaoNova = _notificacaoRepositorio.GetById(notificacaoRetorno);
            var negociacaoSeloDescontoNotificacao = new NegociacaoSeloDescontoNotificacao
            {
                Notificacao = notificacaoNova,
                PedidoSelo = pedido
            };

            Save(negociacaoSeloDescontoNotificacao);
        }
    }
}