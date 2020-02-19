using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class FaturamentoMap : ClassMap<Faturamento>
    {
        public FaturamentoMap()
        {
            Table("Faturamento");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            Map(x => x.DataInsercao).Column("DataInsercao");

            Map(x => x.IdSoftpark).Column("IdSoftpark");
            Map(x => x.NomeUnidade, "NomeUnidade");
            Map(x => x.NumFechamento, "NumFechamento");
            Map(x => x.NumTerminal, "NumTerminal");
            Map(x => x.DataAbertura, "DataAbertura");
            Map(x => x.DataFechamento, "DataFechamento");
            Map(x => x.TicketInicial, "TicketInicial");
            Map(x => x.TicketFinal, "TicketFinal");
            Map(x => x.PatioAtual, "PatioAtual");
            Map(x => x.ValorTotal, "ValorTotal");
            Map(x => x.ValorRotativo, "ValorRotativo");
            Map(x => x.ValorRecebimentoMensalidade, "ValorRecebimentoMensalidade");
            Map(x => x.ValorDinheiro, "ValorDinheiro");
            Map(x => x.ValorCartaoDebito, "ValorCartaoDebito");
            Map(x => x.ValorCartaoCredito, "ValorCartaoCredito");
            Map(x => x.ValorSemParar, "ValorSemParar");
            Map(x => x.ValorSeloDesconto, "ValorSeloDesconto");
            Map(x => x.SaldoInicial, "SaldoInicial");
            Map(x => x.ValorSangria, "ValorSangria");

            References(x => x.Unidade, "Unidade_Id");
            References(x => x.Usuario, "Usuario_Id");
        }
    }
}
