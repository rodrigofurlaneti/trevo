using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    class PrecoStatusMap : ClassMap<PrecoStatus>
    {
        public PrecoStatusMap()
        {
            Table("PrecoStatus");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            Map(x => x.Descricao).Column("Nome").Not.Nullable().Length(30);
            Map(x => x.DataInsercao).Column("DataInsercao").Not.Nullable();
        }
    }
}
