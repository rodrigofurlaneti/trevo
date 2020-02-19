using Entidade.Base;
using System.ComponentModel.DataAnnotations;

namespace Entidade
{
    public class Marca : BaseEntity
    {
        [Required(ErrorMessage = "*")]
        [MaxLength(255)]
        public virtual string Nome { get; set; }
    }
}