using Entidade.Base;
using System;
using System.ComponentModel.DataAnnotations;

namespace Entidade
{
    public class TipoMensalista : BaseEntity
    {
        //public virtual int Id { get; set; }
        [Required]
        public virtual string Descricao { get; set; }
        public virtual bool Ativo { get; set; }
    }
}