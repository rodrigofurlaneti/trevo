using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class ModeloMap : ClassMap<Modelo>
    {
        public ModeloMap()
        {
            Table("Modelo");
            LazyLoad();

            Id(x => x.Id).GeneratedBy.Identity().Column("Id");

            Map(x => x.Descricao).Column("Descricao").Not.Nullable();

            References(x => x.Marca);

        }
    }
}