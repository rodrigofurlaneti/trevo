using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class ConsolidaAjusteFinalFaturamentoMap : ClassMap<ConsolidaAjusteFinalFaturamento>
    {
        public ConsolidaAjusteFinalFaturamentoMap()
        {
            Table("ConsolidaAjusteFinalFaturamento");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            Map(x => x.DataInsercao).Column("DataInsercao");
            Map(x => x.AjusteFinalFaturamento).Column("AjusteFinalFaturamento");
            Map(x => x.DespesaFinal).Column("DespesaFinal");
            Map(x => x.Diferenca).Column("Diferenca");
            Map(x => x.FaturamentoFinal).Column("FaturamentoFinal");
        }
    }
}
