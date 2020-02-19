using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class HorarioPrecoMap : ClassMap<HorarioPreco>
    {
        public HorarioPrecoMap()
        {
            Table("HorariosPrecos");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            Map(x => x.DataInsercao).Column("DataInsercao").Not.Nullable();
            Map(x => x.Horario).Column("Horario").Not.Nullable().Length(100);
            Map(x => x.Valor).Column("Valor").Length(10).Precision(10).Scale(2).Not.Nullable();
        }
    }
}
