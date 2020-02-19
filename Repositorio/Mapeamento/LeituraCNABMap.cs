using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class LeituraCNABMap : ClassMap<LeituraCNAB>
    {
        public LeituraCNABMap()
        {
            Table("LeituraCNAB");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            
            Map(x => x.DataInsercao).Column("DataInsercao");
            Map(x => x.NomeArquivo).Column("NomeArquivo");

            Map(x => x.CodigoBanco).Column("CodigoBanco");
            Map(x => x.Agencia).Column("Agencia");
            Map(x => x.Conta).Column("Conta");
            Map(x => x.DACConta).Column("DACConta");
            Map(x => x.NumeroCNAB).Column("NumeroCNAB");
            Map(x => x.ValorTotal).Column("ValorTotal");
            Map(x => x.DataGeracao).Column("DataGeracao");
            Map(x => x.DataCredito).Column("DataCredito");

            Map(x => x.Arquivo).Column("Arquivo").Length(int.MaxValue).Nullable();

            References(x => x.ContaFinanceira).Column("ContaFinanceira").Nullable().Cascade.None();

            HasMany(x => x.ListaLancamentos)
                .Table("LeituraCNABLancamentoCobranca")
                .KeyColumn("LeituraCNAB")
                .Component(m =>
                {
                    m.References(x => x.LancamentoCobranca).Column("LancamentoCobranca_id").Cascade.None();
                }).Cascade.None();
        }
    }
}