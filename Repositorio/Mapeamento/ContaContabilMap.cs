using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class ContaContabilMap : ClassMap<ContaContabil>
    {
        public ContaContabilMap()
        {
            Table("ContaContabil");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("Id");

            Map(x => x.Descricao).Column("Descricao");
            Map(x => x.DataInsercao).Column("DataInsercao");
            Map(x => x.Ativo).Column("Ativo").Not.Nullable().Default("1");
            Map(x => x.Fixa).Column("Fixa").Not.Nullable().Default("1");
            Map(x => x.Hierarquia).Column("Hierarquia").Not.Nullable();
            Map(x => x.Despesa).Column("Despesa");
            References(x => x.ContaContabilPai).Column("ContaContabilPai");
        }
    }
}