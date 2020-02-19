using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    class ParametroNotificacaoMap : ClassMap<ParametroNotificacao>
    {
        public ParametroNotificacaoMap()
        {
            Table("ParametroNotificacao");
            LazyLoad();

            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            Map(x => x.DataInsercao).Column("DataInsercao").Not.Nullable();

            References(x => x.TipoNotificacao).Column("TipoNotificacao").Cascade.None();

            //HasMany(x => x.Aprovadores)
            //    .Table("ParametroNotificacaoUsuario")
            //    .KeyColumn("ParametroNotificacao")
            //    .Component(m =>
            //    {
            //        m.References(x => x.Usuario).Column("Usuario");
            //    }).Cascade.All();

            HasMany(x => x.Aprovadores).Cascade.AllDeleteOrphan();
        }
    }

    class ParametroNotificacaoUsuarioMap : ClassMap<ParametroNotificacaoUsuario>
    {
        public ParametroNotificacaoUsuarioMap()
        {
            Table("ParametroNotificacaoUsuario");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("Id");

            References(x => x.Usuario).Column("Usuario");
            References(x => x.ParametroNotificacao).Column("ParametroNotificacao");
        }
    }

}
