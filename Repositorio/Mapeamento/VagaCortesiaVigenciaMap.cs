using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class VagaCortesiaVigenciaMap : ClassMap<VagaCortesiaVigencia>
    {
        public VagaCortesiaVigenciaMap()
        {
            Table("VagaCortesiaVigencia");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("Id");

            References(x => x.Unidade).Column("Unidade");
            Map(x => x.DataInicio).Column("DataInicio");
            Map(x => x.DataFim).Column("DataFim");
            Map(x => x.HorarioInicio).Column("HorarioInicio");
            Map(x => x.HorarioFim).Column("HorarioFim");

            References(x => x.VagaCortesia).Column("VagaCortesia");
        }
    }
}