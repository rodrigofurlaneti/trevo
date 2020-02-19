using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class ItemFuncionarioMap : ClassMap<ItemFuncionario>
    {
        public ItemFuncionarioMap()
        {
            Table("ItemFuncionario");
            LazyLoad();

            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            Map(x => x.DataInsercao).Column("DataInsercao").Not.Nullable().Not.Update();
            Map(x => x.DataEntrega).Nullable();
            Map(x => x.DataDevolucao).Nullable();

            References(x => x.Funcionario, "Funcionario").Unique();
            References(x => x.ResponsavelEntrega, "ResponsavelEntrega").Nullable().Cascade.None();
            References(x => x.ResponsavelDevolucao, "ResponsavelDevolucao").Nullable().Cascade.None();
            HasMany(x => x.ItemFuncionariosDetalhes).Cascade.AllDeleteOrphan();
        }
    }
}