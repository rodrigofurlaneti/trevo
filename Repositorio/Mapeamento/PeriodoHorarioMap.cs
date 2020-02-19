using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class PeriodoHorarioMap : ClassMap<PeriodoHorario>
    {
        public PeriodoHorarioMap()
        {
            Table("PeriodoHorario");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("Id");

            Map(x => x.TipoHorario).Column("TipoHorario");

            Map(x => x.Periodo).Column("Periodo");
            Map(x => x.Inicio).Column("Inicio");
            Map(x => x.Fim).Column("Fim");

        }
    }
}