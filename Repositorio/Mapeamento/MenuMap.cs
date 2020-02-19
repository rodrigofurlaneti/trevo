using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class MenuMap : ClassMap<Menu>
    {
        public MenuMap()
        {
            Table("Menu");
            LazyLoad();

            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            Map(x => x.DataInsercao).Column("DataInsercao").Not.Nullable();
            Map(x => x.Descricao).Column("Descricao").Not.Nullable();
            Map(x => x.Url).Column("Url");
            Map(x => x.Posicao).Column("Posicao");
            Map(x => x.Ativo).Column("Ativo").Not.Nullable().Default("1");

            References(x => x.MenuPai).Column("MenuPai");
        }
    }
}
