using Entidade.Base;
using System.ComponentModel.DataAnnotations;

namespace Entidade
{
    public class Cargo : BaseEntity
    {
        [Required(ErrorMessage = "*")]
        public virtual string Nome { get; set; }
    }
}
