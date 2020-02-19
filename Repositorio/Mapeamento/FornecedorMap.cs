using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class FornecedorMap : SubclassMap<Fornecedor>
    {
        public FornecedorMap()
        {
            Table("Fornecedor");
            LazyLoad();
            KeyColumn("Pessoa");

            Map(x => x.NomeFantasia).Column("NomeFantasia");
            Map(x => x.RazaoSocial).Column("RazaoSocial");
            Map(x => x.TipoPessoa).Column("TipoPessoa").Not.Nullable();
            Map(x => x.ReceberCotacaoPorEmail).Not.Nullable();

            Map(x => x.Agencia).Column("Agencia").Nullable();
            Map(x => x.DigitoAgencia).Column("DigitoAgencia").Nullable();
            Map(x => x.Conta).Column("Conta").Nullable();
            Map(x => x.DigitoConta).Column("DigitoConta").Nullable();
            Map(x => x.CPFCNPJ).Column("CPFCNPJ").Nullable();
            Map(x => x.Beneficiario).Column("Beneficiario").Nullable();

            References(x => x.Banco).Column("Banco").Nullable().Cascade.None();
        }
    }
}