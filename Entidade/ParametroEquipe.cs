using Entidade.Base;
using Entidade.Uteis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidade
{
    public class ParametroEquipe : BaseEntity
    {
        public virtual bool Ativo { get; set; }
        public virtual Equipe Equipe { get; set; }
        public virtual IList<HorarioParametroEquipe> HorarioParametroEquipe { get; set; }
        public virtual StatusSolicitacao Status { get; set; }
        public virtual string Usuario { get; set; }

        public virtual IList<ParametroEquipeNotificacao> Notificacoes { get; set; }
    }
}
