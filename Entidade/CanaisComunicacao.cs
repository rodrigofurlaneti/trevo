using Entidade.Base;
using Entidade.Uteis;
using System.ComponentModel.DataAnnotations;

namespace Entidade
{
    public class CanaisComunicacao : BaseEntity
    {
        [Required(ErrorMessage = "*")]
        public virtual TipoComunicacao TipoComunicacao { get; set; }

        [Required(ErrorMessage = "*")]
        public virtual CanalComunicacao CanalComunicacao { get; set; }
        
    }
}
