using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class CalculoFechamentoMap : ClassMap<CalculoFechamento>
    {
        public CalculoFechamentoMap()
        {
            Table("CalculoFechamento");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            Map(x => x.DataInsercao).Column("DataInsercao").Not.Nullable();
            Map(x => x.PrefeituraComplementarMaiorIgualDespesa).Column("PrefeituraComplementarMaiorIgualDespesa");
            Map(x => x.PrefeituraMaiorIgualCartao).Column("PrefeituraMaiorIgualCartao");
            Map(x => x.ValorComplementarEmitido).Column("ValorComplementarEmitido");
            Map(x => x.ValorNotaEmissao).Column("ValorNotaEmissao");
            References(x => x.AjusteFaturamento).Column("AjusteFaturamento");
        }
    }
}
