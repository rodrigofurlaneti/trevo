using Entidade.Base;
using System;
using System.ComponentModel.DataAnnotations;

namespace Entidade
{
    public class CheckListEstruturaUnidade : BaseEntity
    {
        [Required]

        public virtual string EstruturaGaragem { get; set; }
        public virtual int Quantidade { get; set; }
        public virtual bool Ativo { get; set; }
    }
}