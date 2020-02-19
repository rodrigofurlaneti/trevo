using System.ComponentModel.DataAnnotations;
using Entidade.Base;

namespace Entidade
{
    public class Pais : BaseEntity
    {
        [Required]
        public virtual string Descricao { get; set; }
    }
}