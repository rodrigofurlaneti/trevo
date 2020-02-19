using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidade
{
    public class EquipeColaborador
    {
        public virtual Colaborador Colaborador { get; set; }
        public virtual Equipe Equipe { get; set; }
    }
}
