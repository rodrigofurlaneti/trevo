using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using Entidade.Base;

namespace Entidade
{
    public class ClienteUnidade : BaseEntity
    {
        public virtual Unidade Unidade { get; set; }
        public int Cliente { get; set; }
    }
}