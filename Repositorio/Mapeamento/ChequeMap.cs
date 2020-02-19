using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class ChequeMap : ClassMap<Cheque>
    {
        public ChequeMap()
        {
            Table("Cheque");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            Map(x => x.Numero).Column("Numero").Not.Nullable();
            Map(x => x.Emitente).Column("Descricao").Not.Nullable();
            Map(x => x.Agencia).Column("Agencia").Not.Nullable();
            Map(x => x.DigitoAgencia).Column("DigitoAgencia");
            Map(x => x.Conta).Column("Conta").Not.Nullable();
            Map(x => x.DigitoConta).Column("DigitoConta").Not.Nullable();
            Map(x => x.CPFCNPJ).Column("CPFCNPJ");

            //novos campos
            Map(x => x.DataDevolucao).Column("DataDevolucao").Nullable();
            Map(x => x.MotivoDevolucao).Column("MotivoDevolucao").Nullable();

            Map(x => x.DataProtesto).Column("DataProtesto").Nullable();
            Map(x => x.CartorioProtestado).Column("CartorioProtestado").Nullable();

            Map(x => x.StatusCheque).Column("StatusCheque").Nullable();

            Map(x => x.Valor).Column("Valor").Length(10).Precision(10).Scale(2).Not.Nullable();

            References(x => x.ContaFinanceira).Column("Banco");
            References(x => x.Banco).Column("BancoCheque");
        }
    }
}
