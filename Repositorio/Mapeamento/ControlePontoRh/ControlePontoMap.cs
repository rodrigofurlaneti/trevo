using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class ControlePontoMap : ClassMap<ControlePonto>
    {
        public ControlePontoMap()
        {
            Table("ControlePonto");
            LazyLoad();

            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            Map(x => x.DataInsercao).Column("DataInsercao").Not.Nullable().Not.Update();

            References(x => x.Funcionario, "Funcionario").Not.Nullable().Cascade.None();
            HasMany(x => x.ControlePontoDias).KeyColumn("ControlePontoDia").Cascade.All();
        }
    }
}