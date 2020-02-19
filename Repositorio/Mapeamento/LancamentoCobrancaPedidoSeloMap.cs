using Entidade;
using FluentNHibernate.Mapping;

namespace Repositorio.Mapeamento
{
    public class LancamentoCobrancaPedidoSeloMap : SubclassMap<LancamentoCobrancaPedidoSelo>
    {
        public LancamentoCobrancaPedidoSeloMap()
        {
            Table("LancamentoCobrancaPedidoSelo");
            LazyLoad();
            References(x => x.PedidoSelo).Column("PedidoSelo");
        }
    }
}