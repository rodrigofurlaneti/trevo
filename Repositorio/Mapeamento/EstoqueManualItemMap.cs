using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class EstoqueManualItemMap : ClassMap<EstoqueManualItem>
    {
        public EstoqueManualItemMap()
        {
            Table("EstoqueManualItem");

            LazyLoad();

            Id(x => x.Id).GeneratedBy.Identity().Column("Id");

            Map(x => x.DataInsercao).Column("DataInsercao").Not.Nullable();
            Map(x => x.CodigoMaterial).Column("CodigoMaterial").Nullable();
            Map(x => x.NumeroMaterial).Column("NumeroMaterial").Nullable();
            Map(x => x.Inventariado).Nullable();

            References(x => x.Estoque).Column("Estoque").Cascade.None();
            References(x => x.Unidade).Column("Unidade").Cascade.None();
            References(x => x.Material).Column("Material").Cascade.None();
        }
    }
}