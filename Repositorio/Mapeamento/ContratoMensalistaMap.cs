using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class ContratoMensalistaMap : ClassMap<ContratoMensalista>
    {
        public ContratoMensalistaMap()
        {
            Table("ContratoMensalista");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            Map(x => x.DataInsercao).Column("DataInsercao").Not.Nullable();
            Map(x => x.DataVencimento).Column("DataVencimento").Nullable();
            Map(x => x.Valor).Column("Valor").Length(10).Precision(10).Scale(2);
            Map(x => x.DataInicio).Column("DataInicio").Nullable();
            Map(x => x.DataFim).Column("DataFim").Nullable();
            Map(x => x.Ativo).Column("Ativo").Default("1");
            Map(x => x.NumeroContrato).Column("NumeroContrato");
            Map(x => x.NumeroVagas).Column("NumeroVagas");
            Map(x => x.Frota).Column("Frota");
            Map(x => x.HorarioInicio).Column("HorarioInicio").Nullable();
            Map(x => x.HorarioFim).Column("HorarioFim").Nullable();
            Map(x => x.Observacao).Column("Observacao").Nullable();

            References(x => x.Cliente).Column("Cliente_id").Cascade.None();
            References(x => x.TipoMensalista).Column("TipoMensalista_id").Cascade.All();
            References(x => x.Unidade).Columns("Unidade_id").Cascade.None();
            References(x => x.TabelaPrecoMensalista).Column("TabelaPrecoMensalista").Cascade.None();

			HasMany(x => x.Veiculos)
				.Table("ContratoMensalistaVeiculo")
				.KeyColumn("ContratoMensalista_id")
				.Component(m =>
				{
					m.References(x => x.Veiculo).Column("Veiculo_id").Cascade.None();
				}).Cascade.AllDeleteOrphan();

            HasMany(x => x.ContratoMensalistaNotificacoes)
                .Table("ContratoMensalistaNotificao")
                .KeyColumn("ContratoMensalista")
                .Component(m =>
                {
                    m.References(x => x.Notificacao).Cascade.All();
                }).Cascade.AllDeleteOrphan();
        }
    }
}