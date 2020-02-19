using Entidade.Base;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace Entidade
{
    public class CheckListAtividade : BaseEntity
    {
        [Required]
        public virtual string Descricao { get; set; }
        public virtual bool Ativo { get; set; }
        public virtual string Usuario { get; set; }
        public virtual Funcionario Responsavel { get; set; }
        public virtual IList<CheckListAtividadeTipoAtividade> TiposAtividade { get; set; }
    }
}