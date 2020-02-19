using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class MarcaMap : ClassMap<Marca>
    {
        public MarcaMap()
        {
            Table("Marca");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            Map(x => x.Nome).Column("Nome");
            
        }
    }
}
