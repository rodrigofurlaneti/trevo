using Entidade.Base;

namespace Entidade
{
    public class HorarioUnidadePeriodoHorario : BaseEntity
    {
        public virtual PeriodoHorario PeriodoHorario { get; set; }
        public virtual int DiasPeriodo { get; set; }
        public int HorarioUnidade { get; set; }
    }
}
