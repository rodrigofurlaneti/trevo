using Core.Exceptions;
using Entidade.Base;
using Entidade.Uteis;
using System;
using System.Collections.Generic;

namespace Entidade
{
    public class Notificacao : BaseEntity
    {
        // Criador da notificação
        public virtual Usuario Usuario { get; set; }
        public virtual Usuario Aprovador { get; set; }
        public virtual DateTime DataAprovacao { get; set; }
        public virtual StatusNotificacao Status { get; set; }
        public virtual TipoNotificacao TipoNotificacao { get; set; }
        public virtual string Descricao { get; set; }
        public virtual string UrlPersonalizada { get; set; }

        /// <summary>
        /// Usuários que podem aprovar
        /// </summary>
        public virtual IList<NotificacaoUsuarioAprovador> NotificacaoUsuarioAprovadores { get; set; }

        public virtual DateTime? DataVencimentoNotificacao { get; set; }
        public virtual TipoAcaoNotificacao AcaoNotificacao { get; set; }

        public Notificacao()
        {
            DataAprovacao = System.Data.SqlTypes.SqlDateTime.MinValue.Value;
        }

        public virtual void Aprovar(Usuario usuario)
        {
            ValidarAprovacao();

            DataAprovacao = DateTime.Now;
            Aprovador = usuario;
            Status = StatusNotificacao.Aprovado;
        }

        public virtual void Reprovar(Usuario usuario)
        {
            ValidarReprovacao();

            DataAprovacao = DateTime.Now;
            Aprovador = usuario;
            Status = StatusNotificacao.Reprovado;
        }

        public virtual void ValidarAprovacao()
        {
            if (Status == StatusNotificacao.Reprovado)
                throw new BusinessRuleException("A notificação encontra-se 'Reprovada'. Não é possível 'Aprovar'!");
        }

        public virtual void ValidarReprovacao()
        {
            if (Status == StatusNotificacao.Aprovado)
                throw new BusinessRuleException("A notificação encontra-se 'Aprovada'. Não é possível 'Reprovar'!");
        }
    }
}