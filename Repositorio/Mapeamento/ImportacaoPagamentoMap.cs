using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class ImportacaoPagamentoMap : ClassMap<ImportacaoPagamento>
    {
        public ImportacaoPagamentoMap()
        {
            Table("ImportacaoPagamento");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("Id");

            Map(x => x.DataInsercao).Column("DataInsercao");
            Map(x => x.Arquivo).Column("Arquivo");
            Map(x => x.Lote).Column("Lote");
            Map(x => x.DataPagamento).Column("DataPagamento");
            Map(x => x.Cedente).Column("Cedente");

            References(x => x.Usuario).Cascade.None();
            HasMany(x => x.Pagamento).Cascade.None();
        }
    }
}