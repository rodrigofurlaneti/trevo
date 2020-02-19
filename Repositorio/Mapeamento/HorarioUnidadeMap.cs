using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class HorarioUnidadeMap : ClassMap<HorarioUnidade>
    {
        public HorarioUnidadeMap()
        {
            Table("HorarioUnidade");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("Id");

            Map(x => x.Nome).Column("Nome");
            Map(x => x.Fixo).Column("Fixo").Not.Nullable().Default("1");
            Map(x => x.DataValidade).Column("DataValidade");
            Map(x => x.TipoHorario).Column("TipoHorario");
            Map(x => x.Feriados).Column("Feriados").Not.Nullable().Default("1");
            Map(x => x.Status).Column("Status").Not.Nullable();
            References(x => x.Unidade).Column("Unidade").Cascade.None();

            HasMany(x => x.PeriodosHorario)
              .Table("HorarioUnidadePeriodoHorario")
              .KeyColumn("PeriodoHorario")
              .Component(m =>
              {
                  m.References(x => x.PeriodoHorario).Cascade.All();
              }).Cascade.All();

            //HasMany(x => x.Notificacoes)
            //    .Table("HorarioUnidadeNotificacao")
            //    .KeyColumn("HorarioUnidade")
            //    .Component(m =>
            //    {
            //        m.References(x => x.Notificacao).Column("Notificacao").Cascade.All();
            //    }).Cascade.AllDeleteOrphan();

            //HasMany(x => x.Notificacoes).Cascade.AllDeleteOrphan();
        }
    }


}