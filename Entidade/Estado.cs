using System.ComponentModel.DataAnnotations;
using Core.Attributes;
using Entidade.Base;

namespace Entidade
{
    public class Estado : BaseEntity
    {
        [Required]
        public virtual string Descricao { get; set; }

        public virtual string Sigla { get; set; }

        [NotExportField]
        public virtual Pais Pais { get; set; }
        [NotExportField]
        public virtual bool AssociaMaterialCampanha { get; set; }
    }
}
