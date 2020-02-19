using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class TipoPagamentoMap : ClassMap<TipoPagamento>
    {
        public TipoPagamentoMap()
        {
            Table("TipoPagamento");
            LazyLoad();
            Id(tipoPagamento => tipoPagamento.Id).GeneratedBy.Identity().Column("Id");
            Map(tipoPagamento => tipoPagamento.DataInsercao).Column("DataInsercao").Not.Nullable();
            Map(tipoPagamento => tipoPagamento.Codigo).Column("Codigo").Nullable();
            Map(tipoPagamento => tipoPagamento.Descricao).Column("Descricao").Not.Nullable();
        }
    }
}
