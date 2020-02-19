using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class NecessidadeCompraMaterialFornecedorMap : ClassMap<NecessidadeCompraMaterialFornecedor>
    {
        public NecessidadeCompraMaterialFornecedorMap()
        {
            Table("NecessidadeCompraMaterialFornecedor");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            Map(x => x.DataInsercao).Column("DataInsercao").Not.Nullable();
            Map(x => x.Quantidade).Not.Nullable();

            References(x => x.Fornecedor).Column("Fornecedor_Id").Cascade.None().Not.Nullable();
            References(x => x.Material).Column("Material_Id").Cascade.None().Not.Nullable();
            References(x => x.NecessidadeCompra).Column("NecessidadeCompra_Id").Cascade.None().Nullable();
        }
    }
}