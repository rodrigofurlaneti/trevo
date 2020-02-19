using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class ContasAPagarMap : ClassMap<ContasAPagar>
    {
        public ContasAPagarMap()
        {
            Table("ContasAPagar");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            Map(x => x.TipoPagamento).Column("TipoPagamento").Nullable();
            Map(x => x.DataVencimento).Column("DataVencimento").Not.Nullable();
            Map(x => x.DataCompetencia).Column("DataCompetencia").Nullable();
            Map(x => x.DataPagamento).Column("DataPagamento");
            Map(x => x.ValorTotal).Column("ValorTotal").Length(10).Precision(10).Scale(2).Not.Nullable();
            Map(x => x.FormaPagamento).Column("FormaPagamento").Not.Nullable();
            Map(x => x.NumeroParcela).Column("NumeroParcela").Not.Nullable();
            Map(x => x.Observacoes).Column("Observacoes").Nullable().Length(500);
            Map(x => x.PodePagarEmEspecie).Column("PodePagarEmEspecie").Not.Nullable().Default("0");
            Map(x => x.ValorSolicitado).Column("ValorSolicitado").Not.Nullable().Default("0");
            Map(x => x.StatusConta).Column("StatusConta").Not.Nullable();
            Map(x => x.Ativo).Column("Ativo").Not.Nullable().Default("1");
            Map(x => x.Ignorado).Column("Ignorado");
            Map(x => x.CodigoAgrupadorParcela).Column("CodigoAgrupadorParcela").Not.Nullable().Length(14);
            Map(x => x.NumeroRecibo).Column("NumeroRecibo").Nullable().Length(40);
            Map(x => x.PossueCnab, "PossueCnab").Not.Nullable().Default("0");

            Map(x => x.TipoJuros).Column("TipoJuros").Not.Nullable();
            Map(x => x.ValorJuros).Column("ValorJuros").Length(10).Precision(10).Scale(2).Nullable();
            Map(x => x.TipoMulta).Column("TipoMulta").Not.Nullable();
            Map(x => x.ValorMulta).Column("ValorMulta").Length(10).Precision(10).Scale(2).Nullable();

            Map(x => x.TipoDocumentoConta).Column("TipoDocumentoConta").Nullable();
            Map(x => x.NumeroDocumento).Column("NumeroDocumento").Nullable();

            References(x => x.ContaFinanceira).Column("ContaFinanceira").Cascade.None();
            References(x => x.Departamento).Column("Departamento").Cascade.None();
            References(x => x.Fornecedor).Column("Fornecedor").Cascade.None();
            References(x => x.Unidade).Column("Unidade").Cascade.None();

            Map(x => x.CodigoDeBarras).Column("CodigoDeBarras").Nullable().Length(100);

            HasMany(x => x.ContaPagarItens).Cascade.All();

            HasMany(x => x.ContaPagarNotificacoes)
                .Table("ContaPagarNotificacao")
                .KeyColumn("ContasAPagar_Id")
                .Component(c =>
                {
                    c.References(r => r.Notificacao, "Notificacao_Id").Cascade.All();
                }).Cascade.All();
        }
    }
}