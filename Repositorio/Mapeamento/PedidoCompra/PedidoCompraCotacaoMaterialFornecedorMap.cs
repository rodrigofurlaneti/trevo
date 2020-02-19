using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class PedidoCompraCotacaoMaterialFornecedorMap : ClassMap<PedidoCompraCotacaoMaterialFornecedor>
    {
        public PedidoCompraCotacaoMaterialFornecedorMap()
        {
            Table("PedidoCompraCotacaoMaterialFornecedor");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            Map(x => x.DataInsercao).Column("DataInsercao").Not.Nullable();
            Map(x => x.Selecionado).Not.Nullable();

            References(x => x.PedidoCompra).Column("PedidoCompra_Id").Cascade.None().Nullable();
            References(x => x.CotacaoMaterialFornecedor).Column("CotacaoMaterialFornecedor_Id").Cascade.None().Not.Nullable();
        }
    }
}