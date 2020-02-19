using Entidade.Base;
using Entidade.Uteis;
using System.ComponentModel.DataAnnotations;

namespace Entidade
{
    public class PagamentoReembolso : BaseEntity
    {
        [Required]
        public virtual ContasAPagar ContaAPagar { get; set; }

        [Required]
        public virtual string NumeroRecibo { get; set; }

        [Required]
        public virtual StatusPagamentoReembolso Status { get; set; }
    }
}