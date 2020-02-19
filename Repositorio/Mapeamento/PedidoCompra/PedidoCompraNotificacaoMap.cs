using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class PedidoCompraNotificacaoMap : ClassMap<PedidoCompraNotificacao>
    {
        public PedidoCompraNotificacaoMap()
        {
            Table("PedidoCompraNotificacao");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            Map(x => x.DataInsercao).Column("DataInsercao").Not.Nullable();

            References(x => x.PedidoCompra).Column("PedidoCompra_Id").Cascade.None().Not.Nullable();
            References(x => x.Notificacao).Column("Notificacao_Id").Cascade.All().Not.Nullable();
        }
    }
}