using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class CidadeMap : ClassMap<Cidade>
    {
        public CidadeMap()
        {
            Table("Cidade");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            Map(x => x.Descricao).Column("Descricao").Not.Nullable();
            Map(x => x.DataInsercao).Column("DataInsercao").Not.Nullable();

            References(x => x.Estado).Cascade.None().Not.LazyLoad();
        }
    }
}
