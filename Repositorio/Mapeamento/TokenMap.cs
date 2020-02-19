using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class TokenMap : ClassMap<Token>
    {
        public TokenMap()
        {
            Table("Token");
            LazyLoad();

            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            Map(x => x.DataInsercao).Column("DataInsercao").Not.Nullable();

            Map(x => x.Nome).Column("Nome").Not.Nullable();
            Map(x => x.Descricao).Column("Descricao").Not.Nullable();
        }
    }
}
