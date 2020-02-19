using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class PedidoLocacaoLancamentoAdicionalMap : ClassMap<PedidoLocacaoLancamentoAdicional>
    {
        public PedidoLocacaoLancamentoAdicionalMap()
        {
            Table("PedidoLocacaoLancamentoAdicional");
            LazyLoad();

            Id(x => x.Id).GeneratedBy.Identity().Column("Id");
            Map(x => x.DataInsercao).Column("DataInsercao");
            
            Map(x => x.Valor).Column("Valor").Length(10).Precision(10).Scale(2);
            Map(x => x.Descricao).Column("Descricao");
            Map(x => x.Ativo).Column("Ativo");

            References(x => x.PedidoLocacao).Column("PedidoLocacaoId");
        }
    }
}