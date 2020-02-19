using Entidade.Base;
using System;
using System.ComponentModel.DataAnnotations;

namespace Entidade
{
    public class TipoAtividade : BaseEntity
    {
        [Required]
        public virtual string Descricao { get; set; }
        public virtual bool Ativo { get; set; }
        public virtual string Usuario { get; set; }
    }
}