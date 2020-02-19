using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Entidade.Base;
using Entidade.Uteis;

namespace Entidade
{
    public class BeneficioFuncionario : BaseEntity
    {
        public virtual Funcionario Funcionario { get; set; }
        public virtual IList<BeneficioFuncionarioDetalhe> BeneficioFuncionarioDetalhes { get; set; }
    }
}