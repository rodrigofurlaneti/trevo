using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class PerfilMap : ClassMap<Perfil>
    {
        public PerfilMap()
        {
            Table("Perfil");
            LazyLoad();

            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            Map(x => x.Nome).Column("Nome");
            Map(x => x.DataInsercao).Column("DataInsercao").Not.Nullable();

            HasMany(x => x.Menus)
                .Table("PerfilMenu")
                .KeyColumn("Perfil")
                .Component(m =>
                {
                    m.References(x => x.Menu).Cascade.None();
                });
            
            HasMany(x => x.Usuarios)
                .Table("UsuarioPerfil")
                .KeyColumn("Perfil_id")
                .Component(m =>
                {
                    m.References(x => x.Usuario).Column("Usuario").Cascade.None();
                });

            HasManyToMany(x => x.Permissoes)
                .Table("PerfilPermissao");
        }
    }
}
