using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class NotificacaoMap : ClassMap<Notificacao>
    {
        public NotificacaoMap()
        {
            Table("Notificacao");
            LazyLoad();

            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            Map(x => x.DataInsercao).Column("DataInsercao").Not.Nullable();
            Map(x => x.Status).Column("Status").Not.Nullable();
            Map(x => x.DataAprovacao).Column("DataAprovacao");
            Map(x => x.Descricao).Column("Descricao").Not.Nullable();
            Map(x => x.DataVencimentoNotificacao).Column("DataVencimentoNotificacao").Nullable();
            Map(x => x.AcaoNotificacao).Column("AcaoNotificacao").Default("1").Not.Nullable();
            Map(x => x.UrlPersonalizada).Nullable();

            References(x => x.Usuario).Column("Usuario").Not.Nullable().Cascade.None();
            References(x => x.Aprovador).Column("UsuarioAprovador").Cascade.None();
            References(x => x.TipoNotificacao).Column("TipoNotificacao").Not.Nullable().Cascade.None();
            
            HasMany(x => x.NotificacaoUsuarioAprovadores)
                .Table("NotificacaoUsuarioAprovador")
                .KeyColumn("Notificacao")
                .ForeignKeyConstraintName("FK_Notificacao_To_NotificacaoUsuarioAprovador")
                .Component(c =>
                {
                    c.References(r => r.UsuarioAprovador).Cascade.None();
                }).Cascade.All();
        }
    }
}
