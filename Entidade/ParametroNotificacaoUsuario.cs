using Entidade.Base;
using System.Collections.Generic;

namespace Entidade
{
    public class ParametroNotificacaoUsuario : BaseEntity
    {
        public virtual ParametroNotificacao ParametroNotificacao { get; set; }
        public virtual Usuario Usuario { get; set; }
    }
}