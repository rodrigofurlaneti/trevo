using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using Core.Attributes;
using Entidade.Base;
using Entidade.Uteis;

namespace Entidade
{
    public class Avalista : BaseEntity
    {
        public string Codigo {get ;set;}
        public Pessoa Pessoa {get; set;}
    }
}