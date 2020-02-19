using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    class TipoNotificacaoMap : ClassMap<TipoNotificacao>
    {
        public TipoNotificacaoMap()
        {
            Table("TipoNotificacao");
            LazyLoad();

            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            Map(x => x.DataInsercao).Column("DataInsercao").Not.Nullable();
            Map(x => x.Descricao).Column("Descricao").Not.Nullable();
            Map(x => x.Entidade).Column("Entidade").Not.Nullable();
        }
    }
}
