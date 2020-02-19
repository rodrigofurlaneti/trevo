using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class PedidoLocacaoNotificacaoMap : ClassMap<PedidoLocacaoNotificacao>
    {
        public PedidoLocacaoNotificacaoMap()
        {
            Table("PedidoLocacaoNotificacao");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("Id");

            References(x => x.Notificacao).Column("Notificacao").Cascade.All();
            References(x => x.PedidoLocacao).Column("PedidoLocacao");
        }
    }
}