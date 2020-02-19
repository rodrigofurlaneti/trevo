using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class RetiradaCofreMap : ClassMap<RetiradaCofre>
    {
        public RetiradaCofreMap()
        {
            Table("RetiradaCofre");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            Map(x => x.StatusRetiradaCofre).Column("StatusRetiradaCofre");
            Map(x => x.DataInsercao).Column("DataInsercao");
            Map(x => x.Observacoes).Column("Observacoes");

            References(x => x.ContasAPagar).Column("ContasAPagar").Cascade.SaveUpdate();
            References(x => x.Usuario).Column("Usuario").Cascade.None();

            HasMany(x => x.RetiradaCofreNotificacoes)
                .Table("RetiradaCofreNotificacao")
                .KeyColumn("RetiradaCofre")
                .Component(c =>
                {
                    c.References(r => r.Notificacao, "Notificacao").Cascade.All();
                }).Cascade.All();
        }
    }
}
