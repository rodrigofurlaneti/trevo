using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class ConsolidaDespesaMap : ClassMap<ConsolidaDespesa>
    {
        public ConsolidaDespesaMap()
        {
            Table("ConsolidaDespesa");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            Map(x => x.DataInsercao).Column("DataInsercao");
            Map(x => x.DespesaEscolhida).Column("DespesaEscolhida").Not.Nullable();
            Map(x => x.DespesaEscolhidaFixa).Column("DespesaEscolhidaFixa").Not.Nullable();
            Map(x => x.DespesaFixa).Column("DespesaFixa").Not.Nullable();
            Map(x => x.DespesaTotal).Column("DespesaTotal").Not.Nullable();
            Map(x => x.DespesaValorFinal).Column("DespesaValorFinal").Not.Nullable();
        }
    }
}
