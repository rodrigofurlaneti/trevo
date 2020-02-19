using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class ControlePontoFeriasMap : ClassMap<ControlePontoFerias>
    {
        public ControlePontoFeriasMap()
        {
            Table("ControlePontoFerias");
            LazyLoad();

            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            Map(x => x.DataInsercao).Column("DataInsercao").Not.Nullable().Not.Update();

            References(x => x.Funcionario, "Funcionario").Not.Nullable().Cascade.None();
            HasMany(x => x.ControlePontoFeriasDias).KeyColumn("ControlePontoFeriasDia").Cascade.All();
        }
    }
}