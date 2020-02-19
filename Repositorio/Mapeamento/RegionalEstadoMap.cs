using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class RegionalEstadoMap : ClassMap<RegionalEstado>
    {
        public RegionalEstadoMap()
        {
            Table("RegionalEstado");
            LazyLoad();

            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            References(x => x.Regional).LazyLoad();
            References(x => x.Estado).Not.LazyLoad();
        }
    }
}
