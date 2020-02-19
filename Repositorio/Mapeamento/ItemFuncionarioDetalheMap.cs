using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class ItemFuncionarioDetalheMap : ClassMap<ItemFuncionarioDetalhe>
    {
        public ItemFuncionarioDetalheMap()
        {
            Table("ItemFuncionarioDetalhe");
            LazyLoad();

            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            Map(x => x.DataInsercao).Column("DataInsercao").Not.Nullable().Not.Update();
            Map(x => x.Valor).Scale(2);
            Map(x => x.Quantidade);
            Map(x => x.ValorTotal).Scale(2);

            References(x => x.Material, "Material").Not.Nullable().Cascade.None();
            References(x => x.EstoqueMaterial, "EstoqueMaterial").Not.Nullable().Cascade.None();
        }
    }
}