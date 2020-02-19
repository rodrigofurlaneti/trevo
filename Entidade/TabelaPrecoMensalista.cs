using Entidade.Base;
using Entidade.Uteis;
using System.Collections.Generic;

namespace Entidade
{
    public class TabelaPrecoMensalista : BaseEntity, IAudit
    {
        public virtual string Nome { get; set; }
        public virtual decimal Valor { get; set; }
        public virtual int DiasCalculo { get; set; }
        public virtual StatusSolicitacao Status { get; set; }
        public virtual IList<TabelaPrecoMensalistaUnidade> TabelaPrecoUnidade { get; set; }

        public virtual IList<TabelaPrecoMensalistaNotificacao> Notificacoes { get; set; }
    }
}