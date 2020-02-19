using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class LancamentoCobrancaMap : ClassMap<LancamentoCobranca>
    {
        public LancamentoCobrancaMap()
        {
            Table("LancamentoCobranca");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            Map(x => x.DataInsercao).Column("DataInsercao").Not.Nullable();
            Map(x => x.DataGeracao).Column("DataGeracao").Not.Nullable();
            Map(x => x.DataVencimento).Column("DataVencimento").Not.Nullable();
            Map(x => x.DataCompetencia).Column("DataCompetencia").Nullable();
            Map(x => x.DataBaixa).Column("DataBaixa").Nullable();
            Map(x => x.StatusLancamentoCobranca).Column("StatusLancamentoCobranca").Not.Nullable().Default("1");
            Map(x => x.ValorContrato).Column("ValorContrato").Precision(19).Scale(5).Not.Nullable();
            Map(x => x.TipoValorMulta).Column("TipoValorMulta");
            Map(x => x.ValorMulta).Column("ValorMulta").Precision(19).Scale(5);
            Map(x => x.TipoValorJuros).Column("TipoValorJuros");
            Map(x => x.ValorJuros).Column("ValorJuros").Precision(19).Scale(5);
            Map(x => x.TipoServico).Column("TipoServico").Not.Nullable();
            Map(x => x.PossueCnab).Column("PossueCnab");
            Map(x => x.CiaSeguro).Column("CiaSeguro");
            Map(x => x.NossoNumero).Column("NossoNumero");
            Map(x => x.Observacao).Column("Observacao");
            Map(x => x.TipoOcorrenciaRetorno).Column("TipoOcorrenciaRetorno");

            References(x => x.ContaFinanceira).Column("ContaFinanceira").Cascade.None();
            References(x => x.Cliente).Column("Cliente").Cascade.None();
            References(x => x.Unidade).Column("Unidade").Cascade.None();
            References(x => x.Recebimento).Column("Recebimento").Cascade.All();

            HasMany(x => x.LancamentoCobrancaNotificacoes)
                .Table("LancamentoCobrancaNotificacao")
                .KeyColumn("LancamentoCobranca")
                .Component(c =>
                {
                    c.References(r => r.Notificacao).Cascade.None();
                }).Cascade.SaveUpdate();
        }
    }
}