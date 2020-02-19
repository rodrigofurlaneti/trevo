using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class CargoMap : ClassMap<Cargo>
    {
        public CargoMap()
        {
            Table("Cargo");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            Map(x => x.Nome).Column("Nome");
        }
    }
}