using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class TipoFilialMap : ClassMap<TipoFilial>
    {
        public TipoFilialMap()
        {
            Table("TipoFilial");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            Map(x => x.Nome).Column("Nome");
        }
    }
}
