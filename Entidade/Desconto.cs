using Entidade.Base;
using Entidade.Uteis;
using System;
using System.Collections.Generic;

namespace Entidade
{
    public class Desconto : BaseEntity, IBaseNotificacao
    {
        public virtual string Descricao { get; set; }
        public virtual decimal Valor { get; set; }
        public virtual bool NecessitaAprovacao { get; set; }
        public virtual TipoDesconto TipoDesconto { get; set; }
        public virtual StatusSolicitacao Status { get; set; }

        public virtual IList<NegociacaoSeloDescontoNotificacao> Notificacoes { get; set; }

        public virtual DateTime DataVencimentoNotificacao { get; set; }
    }
}
