using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class PermissaoMap : ClassMap<Permissao>
    {
        public PermissaoMap()
        {
            Table("Permissao");
            LazyLoad();

            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            Map(x => x.Nome).Column("Nome");
            Map(x => x.Regra).Column("Regra");
            Map(x => x.DataInsercao).Column("DataInsercao").Not.Nullable();
            
            HasManyToMany(x => x.Perfis).Table("PerfilPermissao");
        }
    }
}
