using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class NotificacaoAvisoPedidoSeloMap : ClassMap<NotificacaoAvisoPedidoSelo>
    {
        public NotificacaoAvisoPedidoSeloMap()
        {
            Table("NotificacaoAvisoPedidoSelo");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("Id");

            References(x => x.Notificacao).Column("Notificacao").Cascade.None();
            References(x => x.PedidoSelo).Column("PedidoSelo").Cascade.None();
        }
    }
}