using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class EstoqueManualMap : ClassMap<EstoqueManual>
    {
        public EstoqueManualMap()
        {
            Table("EstoqueManual");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            Map(x => x.DataInsercao).Column("DataInsercao");
            Map(x => x.NumeroNFPedido).Column("NumeroNFPedido").Nullable();
            Map(x => x.Quantidade).Column("Quantidade").Nullable();
            Map(x => x.Preco).Column("Preco").Nullable();
            Map(x => x.ValorTotal).Column("ValorTotal").Nullable();
            Map(x => x.Motivo).Column("Motivo").Nullable();
            Map(x => x.Acao).Column("Acao").Nullable();

            References(x => x.PedidoCompra, "PedidoCompra_Id").Cascade.None();
            References(x => x.Estoque).Column("Estoque").Cascade.None();
            References(x => x.Material).Column("Material").Cascade.None();
            References(x => x.Unidade).Column("Unidade").Nullable().Cascade.None();

            HasMany(x => x.ListEstoqueManualItem).Cascade.All();
        }
    }
}