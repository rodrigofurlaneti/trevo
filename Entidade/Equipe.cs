using Entidade.Base;
using Entidade.Uteis;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entidade
{
    public class Equipe : BaseEntity
    {
        public Equipe()
        {
            Datafim = System.Data.SqlTypes.SqlDateTime.MinValue.Value;
            Colaboradores = new List<EquipeColaborador>();
        }
        public virtual string Nome { get; set; }
        public virtual DateTime Datafim { get; set; }
        public virtual bool Ativo { get; set; }
        public virtual Unidade Unidade { get; set; }
        public virtual TipoEquipe TipoEquipe { get; set; }
        public virtual Funcionamento  HorarioTrabalho { get; set; }
        public virtual TipoHorario TipoHorario { get; set; }
        public virtual Funcionario Encarregado { get; set; }
        public virtual Funcionario Supervisor { get; set; }
        public virtual IList<EquipeColaborador> Colaboradores { get; set; }
        public virtual IList<ParametroEquipe> ParametrosEquipe { get; set; }
        //public virtual IList<Colaborador> Colaboradores { get; set; }
    }
}
