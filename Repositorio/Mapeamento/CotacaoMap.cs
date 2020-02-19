using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class CotacaoMap : ClassMap<Cotacao>
    {
        public CotacaoMap()
        {
            Table("Cotacao");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            Map(x => x.DataInsercao).Column("DataInsercao").Not.Nullable();
            Map(x => x.Status).Not.Nullable();

            HasMany(x => x.MaterialFornecedores).Cascade.All();
        }
    }
}