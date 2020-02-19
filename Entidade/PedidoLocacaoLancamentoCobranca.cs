using Entidade.Base;

namespace Entidade
{
    public class PedidoLocacaoLancamentoCobranca : BaseEntity
    {
        public virtual PedidoLocacao PedidoLocacao { get; set; }
        public virtual LancamentoCobranca LancamentoCobranca { get; set; }
    }
}