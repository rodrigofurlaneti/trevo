using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class ConsolidaAjusteFaturamentoMap : ClassMap<ConsolidaAjusteFaturamento>
    {
        public ConsolidaAjusteFaturamentoMap()
        {
            Table("ConsolidaAjusteFaturamento");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity();
            Map(x => x.Mes).Column("Mes");
            Map(x => x.Ano).Column("Ano");
            References(x => x.ConsolidaFaturamento).Column("ConsolidaFaturamento").Cascade.All();
            References(x => x.ConsolidaAjusteFinalFaturamento).Column("ConsolidaAjusteFinalFaturamento").Cascade.All();
            References(x => x.ConsolidaDespesa).Column("ConsolidaDespesa").Cascade.All();
            References(x => x.Unidade).Column("Unidade");
            References(x => x.Empresa).Column("Empresa");
        }
    }
}
