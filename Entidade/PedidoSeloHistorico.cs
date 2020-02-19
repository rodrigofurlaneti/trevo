using Entidade.Base;
using Entidade.Uteis;

namespace Entidade
{
    public class PedidoSeloHistorico : BaseEntity
    {
        public virtual PedidoSelo PedidoSelo { get; set; }
        public virtual Proposta Proposta { get; set; }
        public virtual StatusPedidoSelo StatusPedidoSelo { get; set; }
        public virtual string Descricao { get; set; }
        public virtual Usuario Usuario { get; set; }
    }
}