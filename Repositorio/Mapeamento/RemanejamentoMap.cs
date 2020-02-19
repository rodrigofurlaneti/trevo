using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class RemanejamentoMap : ClassMap<Remanejamento>
    {
        public RemanejamentoMap()
        {
            Table("Remanejamentos");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            Map(x => x.Fixo).Column("Fixo").Not.Nullable();
            Map(x => x.DataFim).Column("DataFim");
            Map(x => x.TipoOpreracao).Column("TipoOpreracao");
            References(x => x.RemanejamentoOrigem).Column("RemanejamentoOrigem").Cascade.All();
            References(x => x.RemanejamentoDestino).Column("RemanejamentoDestino").Cascade.All();
        }
    }
}
