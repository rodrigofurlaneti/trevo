using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class ChequeEmitidoMap : ClassMap<ChequeEmitido>
    {
        public ChequeEmitidoMap()
        {
            Table("ChequeEmitido");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            Map(x => x.Numero).Column("Numero").Not.Nullable();
            Map(x => x.Emitente).Column("Descricao").Not.Nullable();
            Map(x => x.Agencia).Column("Agencia").Nullable();
            Map(x => x.DigitoAgencia).Column("DigitoAgencia").Nullable();
            Map(x => x.Conta).Column("Conta").Nullable();
            Map(x => x.DigitoConta).Column("DigitoConta").Nullable();
            Map(x => x.Cpf).Column("CPFCNPJ");
            Map(x => x.DataEmissao).Column("DataEmissao").Nullable();
            Map(x => x.Valor).Column("Valor").Length(10).Precision(10).Scale(2).Not.Nullable();

            References(x => x.Banco).Column("BancoCheque");
            References(x => x.Fornecedor).Column("Fornecedor");

            HasMany(x => x.ListaContaPagar)
            .Table("ChequeEmitidoContaPagar")
            .KeyColumn("ChequeEmitido")
            .Component(m =>
            {
                m.References(x => x.ContaPagar).Cascade.None();
            }).Cascade.All();


            //HasMany(x => x.ListaContaPagar).Cascade.AllDeleteOrphan();
        }
    }
}
