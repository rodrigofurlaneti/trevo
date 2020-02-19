using Dominio.IRepositorio;
using Entidade;
using Entidade.Uteis;
using Repositorio.Base;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Repositorio
{
    public class PedidoSeloNotificacaoRepositorio : NHibRepository<PedidoSeloNotificacao>, IPedidoSeloNotificacaoRepositorio
    {
        private readonly ITipoNotificacaoRepositorio _tipoNotificacaoRepositorio;
        private readonly INotificacaoRepositorio _notificacaoRepositorio;

        public PedidoSeloNotificacaoRepositorio(NHibContext context,
            ITipoNotificacaoRepositorio tipoNotificacaoRepositorio,
            INotificacaoRepositorio notificacaoRepositorio)
            : base(context)
        {
            _tipoNotificacaoRepositorio = tipoNotificacaoRepositorio;
            _notificacaoRepositorio = notificacaoRepositorio;
        }

        public PedidoSelo BuscarPorIdNotificacao(int idNotificacao)
        {
            var pedidoSeloNotificacao = Session.GetItemBy<PedidoSeloNotificacao>(x => x.Notificacao != null && x.Notificacao.Id == idNotificacao);
            return pedidoSeloNotificacao?.PedidoSelo;
        }

        public void CriarNotificacaoBloqueio(PedidoSelo pedido, int idUsuario)
        {
            CriarNotificacao(pedido, idUsuario, "(Desbloquear Lote)");
        }

        public void Criar(PedidoSelo pedido, int idUsuario)
        {
            CriarNotificacao(pedido, idUsuario);
        }

        private void CriarNotificacao(PedidoSelo pedido, int idUsuario, string descricaoAuxiliar = "")
        {
            var notificacao = new Notificacao
            {
                Usuario = new Usuario { Id = idUsuario },
                TipoNotificacao = _tipoNotificacaoRepositorio.FirstBy(x => x.Entidade == Entidades.PedidoSelo),
                Status = StatusNotificacao.Aguardando,
                Descricao = $"Pedido Selo ID: {pedido.Id.ToString()} {descricaoAuxiliar}",
                DataInsercao = DateTime.Now,
                AcaoNotificacao = TipoAcaoNotificacao.AprovarReprovar,
                DataVencimentoNotificacao = pedido.ValidadePedido
            };
            var notificacaoRetorno = _notificacaoRepositorio.SaveAndReturn(notificacao);
            var notificacaoNova = _notificacaoRepositorio.GetById(notificacaoRetorno);
            var pedidoSeloNotificacao = new PedidoSeloNotificacao
            {
                Notificacao = notificacaoNova,
                PedidoSelo = pedido
            };
            if (pedido.Notificacoes == null || !pedido.Notificacoes.Any())
                pedido.Notificacoes = new List<PedidoSeloNotificacao> { pedidoSeloNotificacao };
            else
                pedido.Notificacoes.Add(pedidoSeloNotificacao);

            Save(pedidoSeloNotificacao);
        }
    }
}