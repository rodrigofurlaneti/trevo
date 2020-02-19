using Entidade.Base;
using Entidade.Uteis;
using System;
using System.Collections.Generic;

namespace Entidade
{
    public class FuncionarioIntervaloNoturno
    {
        public virtual DateTime DataInicial { get; set; }
        public virtual DateTime DataFinal { get; set; }
    }
}