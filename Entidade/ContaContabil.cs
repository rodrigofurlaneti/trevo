using Entidade.Base;
using System;
using System.ComponentModel.DataAnnotations;

namespace Entidade
{
    public class ContaContabil : BaseEntity
    {

        [Required]
        public virtual string Descricao { get; set; }
        public virtual bool Ativo { get; set; }
        public virtual bool Fixa { get; set; }
        public virtual int Hierarquia { get; set; }
        public virtual ContaContabil ContaContabilPai { get; set; }
        public virtual bool Despesa { get; set; } 
    }
}