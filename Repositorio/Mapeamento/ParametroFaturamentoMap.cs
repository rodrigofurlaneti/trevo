using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class ParametroFaturamentoMap : ClassMap<ParametroFaturamento>
    {
        public ParametroFaturamentoMap()
        {
            Table("ParametroFaturamento");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            Map(x => x.DataInsercao).Column("DataInsercao");

            Map(x => x.Descricao).Column("Descricao");
            Map(x => x.Agencia).Column("Agencia");
            Map(x => x.DigitoAgencia).Column("DigitoAgencia");
            Map(x => x.Conta).Column("Conta");
            Map(x => x.DigitoConta).Column("DigitoConta");
            Map(x => x.SaldoInicial).Column("SaldoInicial");
            Map(x => x.Convenio).Column("Convenio");
            Map(x => x.Carteira).Column("Carteira");
            Map(x => x.CodigoTransmissao).Column("CodigoTransmissao");

            References(x => x.Empresa).Cascade.None();
            References(x => x.Banco).Cascade.None();
        }
    }
}