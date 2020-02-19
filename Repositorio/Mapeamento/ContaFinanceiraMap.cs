using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class ContaFinanceiraMap : ClassMap<ContaFinanceira>
    {
        public ContaFinanceiraMap()
        {
            Table("ContaFinanceira");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            Map(x => x.Descricao).Column("Descricao").Not.Nullable();
            Map(x => x.Agencia).Column("Agencia");
            Map(x => x.DigitoAgencia).Column("DigitoAgencia");
            Map(x => x.Conta).Column("Conta");
            Map(x => x.DigitoConta).Column("DigitoConta");
            Map(x => x.Cpf).Column("CPFCNPJ");
            Map(x => x.Cnpj).Column("Cnpj");
            Map(x => x.Convenio).Column("Convenio");
            Map(x => x.Carteira).Column("Carteira");
            Map(x => x.CodigoTransmissao).Column("CodigoTransmissao");
            Map(x => x.ContaPadrao).Column("ContaPadrao");

            Map(x => x.ConvenioPagamento).Column("ConvenioPagamento");

            References(x => x.Banco).Column("Banco");
            References(x => x.Empresa).Column("Empresa").Not.Nullable().Cascade.None();
        }
    }
}
