using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Entidade.Base;

namespace Entidade
{
    public class Permissao : BaseEntity
    {
        public Permissao()
        {
            Perfis = new List<Perfil>();
        }
        
        [Required, StringLength(100)]
        public virtual string Nome { get; set; }
        [Required, StringLength(100)]
        public virtual string Regra { get; set; }
        public virtual IList<Perfil> Perfis { get; set; }
    }
}