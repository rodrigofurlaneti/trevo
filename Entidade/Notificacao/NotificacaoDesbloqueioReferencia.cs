using Entidade.Base;
using Entidade.Uteis;
using System;

namespace Entidade
{
    public class NotificacaoDesbloqueioReferencia : BaseEntity, IAudit
    {
        public virtual int IdRegistro { get; set; }
        public virtual Entidades EntidadeRegistro { get; set; }
        public virtual DateTime DataReferencia { get; set; }
        public virtual bool LiberacaoUtilizada { get; set; }
        public virtual StatusDesbloqueioLiberacao StatusDesbloqueioLiberacao { get; set; }
        public virtual Notificacao Notificacao { get; set; }

        public virtual string NomeArquivoCNABAssociado { get; set; }
    }
}