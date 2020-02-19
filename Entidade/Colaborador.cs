using Entidade.Base;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidade
{
    public class Colaborador : BaseEntity
    {
        public virtual PeriodoHorario Turno { get; set; }
        public virtual Funcionario NomeColaborador { get; set; }
    }
}
