using Entidade.Base;
using System;
using System.Collections.Generic;

namespace Entidade
{
    public class ContratoMensalistaNotificacao
    {
        public virtual ContratoMensalista ContratoMensalista { get; set; }
        public virtual Notificacao Notificacao { get; set; }
    }
}
