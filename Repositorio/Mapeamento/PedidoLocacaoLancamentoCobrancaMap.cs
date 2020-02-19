using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class PedidoLocacaoLancamentoCobrancaMap : ClassMap<PedidoLocacaoLancamentoCobranca>
    {
        public PedidoLocacaoLancamentoCobrancaMap()
        {
            Table("PedidoLocacaoLancamentoCobranca");
            LazyLoad();

            Id(x => x.Id).GeneratedBy.Identity().Column("Id");

            References(x => x.LancamentoCobranca).Column("LancamentoCobrancaId");
            References(x => x.PedidoLocacao).Column("PedidoLocacaoId");
        }
    }
}