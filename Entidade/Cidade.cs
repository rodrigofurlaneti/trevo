using System.ComponentModel.DataAnnotations;
using Core.Attributes;
using Entidade.Base;

namespace Entidade
{
    public class Cidade : BaseEntity
    {
        [NotExportField]
        public virtual Estado Estado { get; set; }
        
        [Required]
        public virtual string Descricao { get; set; }
    }
}
