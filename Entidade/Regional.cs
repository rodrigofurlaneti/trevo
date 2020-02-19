using System.Collections.Generic;
using Entidade.Base;
using System.ComponentModel.DataAnnotations;
using System;

namespace Entidade
{
    public class Regional : IEntity
    {
        public virtual int Id { get; set; }

        [Required(ErrorMessage = "*")]
        public virtual string Nome { get; set; }

        public virtual IList<RegionalEstado> Estados { get; set; }

        public virtual DateTime DataInsercao { get; set; }
    }
}
