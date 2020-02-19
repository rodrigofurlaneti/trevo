using Entidade.Base;
using System.Collections.Generic;

namespace Entidade
{
    public class ParametroNotificacao : BaseEntity
    {
        public virtual TipoNotificacao TipoNotificacao { get; set; }
        public virtual IList<ParametroNotificacaoUsuario> Aprovadores { get; set; }
    }
}