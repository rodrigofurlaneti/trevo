using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Entidade.Base;

namespace Entidade
{
    public class PerfilMenu : BaseEntity
    {
        public virtual Menu Menu { get; set; }
    }
}