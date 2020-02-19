using Entidade.Base;

namespace Entidade
{
    public class LancamentoCobrancaPedidoSelo : LancamentoCobranca, IAudit
    {
        public virtual PedidoSelo PedidoSelo { get; set; }
    }
}