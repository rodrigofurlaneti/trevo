using Entidade.Base;
using Entidade.Uteis;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Entidade
{
    public class OcorrenciaFuncionario : BaseEntity
    {
        public virtual Funcionario Funcionario { get; set; }
        public virtual Usuario UsuarioResponsavel { get; set; }
        public virtual IList<OcorrenciaFuncionarioDetalhe> OcorrenciaFuncionarioDetalhes { get; set; }
        public virtual string ValorTotal => (OcorrenciaFuncionarioDetalhes?.Sum(x => x.TipoOcorrencia.Percentual) ?? 0).ToString("N2");
    }
}