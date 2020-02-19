using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class OrcamentoSinistroCotacaoMap : ClassMap<OrcamentoSinistroCotacao>
    {
        public OrcamentoSinistroCotacaoMap()
        {
            Table("OrcamentoSinistroCotacao");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            Map(x => x.DataInsercao).Column("DataInsercao").Not.Nullable();
            Map(x => x.Status).Column("Status").Not.Nullable();

            Map(x => x.ValorTotal).Column("ValorTotal");
            Map(x => x.QuantidadeTotal).Column("QuantidadeTotal");

            HasMany(x => x.OrcamentoSinistroCotacaoNotificacoes)
                .Table("OrcamentoSinistroCotacaoNotificacao")
                .KeyColumn("OrcamentoSinistroCotacao_Id")
                .Component(c =>
                {
                    c.References(r => r.Notificacao, "Notificacao_Id").Cascade.All();
                }).Cascade.All();

            HasMany(x => x.OrcamentoSinistroCotacaoItens).Cascade.All();
        }
    }
}