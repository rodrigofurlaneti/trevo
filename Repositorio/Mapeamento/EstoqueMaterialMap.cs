using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class EstoqueMaterialMap : ClassMap<EstoqueMaterial>
    {
        public EstoqueMaterialMap()
        {
            Table("EstoqueMaterial");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("Id");

            Map(x => x.Quantidade).Not.Nullable();
            Map(x => x.Preco).Not.Nullable();
            Map(x => x.PrimeiroPreco).Scale(2).Not.Nullable();
            Map(x => x.ValorTotal).Not.Nullable();

            References(x => x.Estoque).Not.Nullable();
            References(x => x.Material).Not.Nullable().Cascade.SaveUpdate();
        }
    }
}