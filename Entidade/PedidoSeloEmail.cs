using Entidade.Base;
using Entidade.Uteis;

namespace Entidade
{
    public class PedidoSeloEmail : BaseEntity
    {
        public virtual PedidoSelo PedidoSelo { get; set; }
        public virtual Proposta Proposta { get; set; }
        public virtual string Email { get; set; }
        public virtual bool Enviado { get; set; }
        public virtual string Descricao { get; set; }
        public virtual TipoEnvioEmailPedidoSelo Tipo { get; set; }
    }
}