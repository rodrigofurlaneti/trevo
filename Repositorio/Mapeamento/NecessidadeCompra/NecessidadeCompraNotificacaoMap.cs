using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class NecessidadeCompraNotificacaoMap : ClassMap<NecessidadeCompraNotificacao>
    {
        public NecessidadeCompraNotificacaoMap()
        {
            Table("NecessidadeCompraNotificacao");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            Map(x => x.DataInsercao).Column("DataInsercao").Not.Nullable();

            References(x => x.NecessidadeCompra).Column("NecessidadeCompra_Id").Cascade.None().Not.Nullable();
            References(x => x.Notificacao).Column("Notificacao_Id").Cascade.None().Not.Nullable();
        }
    }
}