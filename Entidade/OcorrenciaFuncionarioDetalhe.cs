using Entidade.Base;
using Entidade.Uteis;
using System;
using System.Collections.Generic;

namespace Entidade
{
    public class OcorrenciaFuncionarioDetalhe : BaseEntity
    {
        public virtual DateTime DataOcorrencia { get; set; }
        public virtual string Justificativa { get; set; }
        public virtual TipoOcorrencia TipoOcorrencia { get; set; }
        public virtual Unidade Unidade { get; set; }
        public virtual Usuario UsuarioResponsavel { get; set; }
    }
}