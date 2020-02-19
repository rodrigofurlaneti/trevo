using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;

namespace Dominio
{
    public interface IHorarioUnidadePeriodoHorarioServico : IBaseServico<HorarioUnidadePeriodoHorario>
    {

    }

    public class HorarioUnidadePeriodoHorarioServico : BaseServico<HorarioUnidadePeriodoHorario, IHorarioUnidadePeriodoHorarioRepositorio>, IHorarioUnidadePeriodoHorarioServico
    {
    }
}
