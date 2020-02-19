using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class TipoMaterialMap : ClassMap<TipoMaterial>
    {
        public TipoMaterialMap()
        {
            Table("TipoMaterial");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            Map(x => x.DataInsercao).Column("DataInsercao").Not.Nullable();
            Map(x => x.Descricao).Column("Descricao").Not.Nullable();
        }
    }
}