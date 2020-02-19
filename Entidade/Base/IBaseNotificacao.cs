using Entidade.Uteis;
using System;

namespace Entidade.Base
{
    public interface IBaseNotificacao : IEntity
    {
        DateTime DataVencimentoNotificacao { get; set; }
        StatusSolicitacao Status { get; set; }
    }
}