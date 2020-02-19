using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class RegionalMap : ClassMap<Regional>
    {
        public RegionalMap()
        {
            Table("Regional");
            LazyLoad();

            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            Map(x => x.Nome).Column("Nome");

            HasMany(x => x.Estados).Cascade.All().LazyLoad();
        }
    }
}
