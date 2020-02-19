using Entidade.Base;

namespace Entidade
{
    public class FeriasClienteContaCorrente : BaseEntity, IAudit
    {
        public virtual ContaCorrenteCliente ContaCorrente { get; set; }
        public virtual FeriasCliente FeriasCliente { get; set; }
    }
}