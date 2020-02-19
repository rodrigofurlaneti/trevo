using Entidade.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidade
{
    public class RemanejamentoTransferencia : BaseEntity
    {
        public virtual Unidade Unidade { get; set; }
        public virtual TipoEquipe TipoEquipe { get; set; }
        public virtual Equipe Equipe { get; set; }
        public virtual HorarioPreco Horario { get; set; }

    }
}
