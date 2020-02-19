using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class ContasAPagarItemMap : ClassMap<ContasAPagarItem>
    {
        public ContasAPagarItemMap()
        {
            Table("ContasAPagarItem");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            Map(x => x.Valor).Column("Valor").Length(10).Precision(10).Scale(2).Not.Nullable();

            References(x => x.ContaContabil).Cascade.None();
            References(x => x.Unidade).Cascade.None();
        }
    }
}