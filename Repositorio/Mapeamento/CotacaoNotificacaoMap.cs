using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class CotacaoNotificacaoMap : ClassMap<CotacaoNotificacao>
    {
        public CotacaoNotificacaoMap()
        {
            Table("CotacaoNotificacao");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            Map(x => x.DataInsercao).Column("DataInsercao").Not.Nullable();

            References(x => x.Cotacao).Column("Cotacao_Id").Cascade.None().Not.Nullable();
            References(x => x.Notificacao).Column("Notificacao_Id").Cascade.None().Not.Nullable();
        }
    }
}