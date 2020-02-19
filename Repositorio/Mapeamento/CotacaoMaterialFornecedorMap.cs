using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class CotacaoMaterialFornecedorMap : ClassMap<CotacaoMaterialFornecedor>
    {
        public CotacaoMaterialFornecedorMap()
        {
            Table("CotacaoMaterialFornecedor");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            Map(x => x.DataInsercao).Column("DataInsercao").Not.Nullable();
            Map(x => x.Quantidade);
            Map(x => x.Valor);
            Map(x => x.ValorTotal);

            References(x => x.Cotacao).Column("Cotacao_Id").Cascade.None().Nullable();
            References(x => x.Material).Column("Material_Id").Cascade.None().Not.Nullable();
            References(x => x.Fornecedor).Column("Fornecedor_Id").Cascade.None().Not.Nullable();
        }
    }
}