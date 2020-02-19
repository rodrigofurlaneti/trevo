using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class MaterialFornecedorMap : ClassMap<MaterialFornecedor>
    {
        public MaterialFornecedorMap()
        {
            Table("MaterialFornecedor");
            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            Map(x => x.DataInsercao).Column("DataInsercao").Not.Nullable();
            Map(x => x.EhPersonalizado);
            Map(x => x.QuantidadeParaPedidoAutomatico);

            References(x => x.Material).Column("Material_Id").Cascade.None();
            References(x => x.Fornecedor).Column("Fornecedor_Id").Cascade.None();
        }
    }
}