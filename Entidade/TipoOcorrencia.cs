using Entidade.Base;
using Entidade.Uteis;
using System;
using System.Collections.Generic;

namespace Entidade
{
    public class TipoOcorrencia : BaseEntity
    {
        public virtual string Descricao { get; set; }
        public virtual decimal Percentual { get; set; }
    }
}