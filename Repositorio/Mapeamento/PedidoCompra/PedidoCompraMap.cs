using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class PedidoCompraMap : ClassMap<PedidoCompra>
    {
        public PedidoCompraMap()
        {
            Table("PedidoCompra");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            Map(x => x.DataInsercao).Column("DataInsercao").Not.Nullable();

            Map(x => x.FormaPagamento).Not.Nullable();
            Map(x => x.TipoPagamento).Not.Nullable();
            Map(x => x.Status).Not.Nullable();

            References(x => x.Estoque).Column("Estoque_Id").Nullable();
            References(x => x.Unidade).Column("Unidade_Id").Nullable();

            HasMany(x => x.PedidoCompraMaterialFornecedores);
        }
    }
}