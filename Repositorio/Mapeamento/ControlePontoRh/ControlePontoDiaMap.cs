using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class ControlePontoDiaMap : ClassMap<ControlePontoDia>
    {
        public ControlePontoDiaMap()
        {
            Table("ControlePontoDia");
            LazyLoad();

            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            Map(x => x.DataInsercao).Column("DataInsercao").Not.Nullable().Not.Update();

            Map(x => x.Data, "Data");
            Map(x => x.Folga, "Folga");
            Map(x => x.Falta, "Falta");
            Map(x => x.Atraso, "Atraso");
            Map(x => x.Suspensao, "Suspensao");
            Map(x => x.Atestado, "Atestado");
            Map(x => x.FaltaJustificada, "FaltaJustificada");
            Map(x => x.AtrasoJustificado, "AtrasoJustificado");
            Map(x => x.Observacao, "Observacao");
            Map(x => x.HorarioEntrada, "HorarioEntrada");
            Map(x => x.HorarioSaidaAlmoco, "HorarioSaidaAlmoco");
            Map(x => x.HorarioRetornoAlmoco, "HorarioRetornoAlmoco");
            Map(x => x.HorarioSaida, "HorarioSaida");
            Map(x => x.HorasDiaTime, "HorasDiaTime");
            Map(x => x.HorasDia, "HorasDia");
            Map(x => x.HoraExtra, "HoraExtra");
            Map(x => x.HoraAtraso, "HoraAtraso");
            Map(x => x.AdicionalNoturno, "AdicionalNoturno");
            HasMany(x => x.UnidadesApoio)
                .Table("ControlePontoUnidadeApoio")
                .KeyColumn("ControlePontoDia")
                .Component(c => {
                    c.Map(x => x.HorarioEntrada, "HorarioEntrada");
                    c.Map(x => x.HorarioSaida, "HorarioSaida");
                    c.Map(x => x.TipoHoraExtra, "TipoHoraExtra");
                    c.Map(x => x.Data, "Data");
                    c.References(x => x.Unidade, "Unidade").Cascade.None();
                })
                .Cascade.AllDeleteOrphan();
        }
    }
}