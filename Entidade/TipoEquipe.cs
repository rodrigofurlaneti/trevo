using Entidade.Base;
using System;
using System.ComponentModel.DataAnnotations;

namespace Entidade
{
    public class TipoEquipe : BaseEntity
    {
        [Required]
        public virtual string Descricao { get; set; }
        public virtual bool Ativo { get; set; }
    }
}