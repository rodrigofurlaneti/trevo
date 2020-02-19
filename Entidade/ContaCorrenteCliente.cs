using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Entidade.Base;
using Entidade.Uteis;

namespace Entidade
{
    public class ContaCorrenteCliente : BaseEntity
    {
        public virtual Cliente Cliente { get; set; }
        public virtual IList<ContaCorrenteClienteDetalhe> ContaCorrenteClienteDetalhes { get; set; }
    }
}