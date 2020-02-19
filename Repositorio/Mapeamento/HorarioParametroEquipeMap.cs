using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class HorarioParametroEquipeMap : ClassMap<HorarioParametroEquipe>
    {
        public HorarioParametroEquipeMap()
        {
            Table("HorarioParametroEquipe");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("Id");

            References(x => x.Unidade).Column("Unidade").Cascade.None();
            Map(x => x.DataInicio).Column("DataInicio");
            Map(x => x.DataFim).Column("DataFim");
            Map(x => x.HorarioInicio).Column("HorarioInicio");
            Map(x => x.HorarioFim).Column("HorarioFim");

        }
    }
}