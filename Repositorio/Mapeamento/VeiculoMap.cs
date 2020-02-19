using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class VeiculoMap : ClassMap<Veiculo>
    {
        public VeiculoMap()
        {
            Table("Veiculo");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            Map(x => x.DataInsercao).Column("DataInsercao").Not.Nullable();
            Map(x => x.Placa).Column("Placa").Not.Nullable();
            Map(x => x.Cor).Column("Cor").Nullable();
            Map(x => x.Ano).Column("Ano").Nullable();
            Map(x => x.TipoVeiculo).Column("TipoVeiculo").Not.Nullable();
            References(x => x.Modelo).Column("Modelo").Cascade.None();

        }
    }
}
