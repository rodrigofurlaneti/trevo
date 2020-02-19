using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class PrecoMap : ClassMap<Preco>
    {
        public PrecoMap()
        {
            Table("Precos");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            Map(x => x.Nome).Column("Nome").Not.Nullable().Length(100);
            Map(x => x.TempoTolerancia).Column("TempoTolerancia");
            Map(x => x.NomeUsuario).Column("NomeUsuario").Length(100);
            Map(x => x.DataInsercao).Column("DataInsercao").Not.Nullable();
            Map(x => x.Ativo).Column("Ativo").Default("1");
            Map(x => x.PrecoStatus).Column("PrecoStatus").Default("1");
            HasMany(x => x.Funcionamentos).Cascade.All();
            //HasMany(x => x.Mensalistas).Cascade.All();
            //HasMany(x => x.Alugueis).Cascade.All();



            //HasMany(x => x.Notificacoes)
            //    .Table("PrecoNotificacao")
            //    .KeyColumn("Preco")
            //    .Component(m =>
            //    {
            //        m.References(x => x.Notificacao).Column("Notificacao").Cascade.All();
            //    }).Cascade.AllDeleteOrphan();

            //HasMany(x => x.Notificacoes).Cascade.AllDeleteOrphan();
        }
    }
}
