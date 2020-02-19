using Entidade.Base;
using System.ComponentModel.DataAnnotations;

namespace Entidade
{
    public class Grupo : BaseEntity
    {
        [Required(ErrorMessage = "*")]
        public virtual string Nome { get; set; }
    }
}
