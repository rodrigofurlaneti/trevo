using System;
using System.ComponentModel.DataAnnotations;
using Core.Attributes;
using Entidade.Base;
using Entidade.Uteis;

namespace Entidade
{
    public class ParametrosLayout : BaseEntity
    {
        [Required]
        public virtual Carteira Carteira { get; set; }
        //[Required]
        //public virtual ParametrosCarteira ParametrosCarteira { get; set; }
        [Required]
        public virtual Layout Layout { get; set; }
        [Required]
        public virtual FormatoCarga FormatoCarga { get; set; }
        
    }
}