using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class CalendarioRHMap : ClassMap<CalendarioRH>
    {
        public CalendarioRHMap()
        {
            Table("CalendarioRH");
            LazyLoad();

            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            Map(x => x.DataInsercao).Column("DataInsercao").Not.Nullable().Not.Update();
            Map(x => x.Data, "Data");
            Map(x => x.Descricao, "Descricao");
            Map(x => x.DataFixa, "DataFixa");
            Map(x => x.TodasUnidade, "TodasUnidade");

            HasMany(x => x.CalendarioRHUnidades)
                .Table("CalendarioRHUnidade")
                .KeyColumn("CalendarioRH")
                .Component(c =>
                {
                    c.References(x => x.Unidade, "Unidade");
                }).Cascade.AllDeleteOrphan();
        }
    }
}