using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class PagamentoMap : ClassMap<Pagamento>
    {
        public PagamentoMap()
        {
            Table("Pagamento");
            LazyLoad();
            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            Map(x => x.DataInsercao).Column("DataInsercao").Not.Nullable();
            
            Map(x => x.PagamentoMensalistaId).Column("PagamentoMensalistaId").Nullable();
            Map(x => x.NossoNumero).Column("NossoNumero");
            Map(x => x.DataPagamento).Column("DataPagamento");
            Map(x => x.ValorPago).Column("ValorPago");
            Map(x => x.FormaPagamento).Column("FormaPagamento");
            Map(x => x.NumeroRecibo).Column("NumeroRecibo");
            Map(x => x.StatusPagamento).Column("StatusPagamento").Not.Nullable().Default("0");

            Map(x => x.StatusEmissao).Column("StatusEmissao").Nullable();
            Map(x => x.DataEnvio).Column("DataEnvio").Nullable();

            Map(x => x.TipoDescontoAcrescimo).Column("TipoDescontoAcrescimo").Nullable().CustomType(typeof(Entidade.Uteis.TipoDescontoAcrescimo));
            Map(x => x.ValorDivergente).Column("ValorDesconto").Nullable();
            Map(x => x.Justificativa).Column("JustificativaDesconto").Nullable().CustomSqlType("varchar(max)");

            References(x => x.Recebimento).Column("Recebimento").Not.Cascade.All();
            References(x => x.Unidade).Column("Unidade").Cascade.None();
            References(x => x.ContaContabil).Column("ContaContabil").Cascade.None();
        }
    }
}