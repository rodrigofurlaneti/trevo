using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;

namespace Dominio
{
    public interface IPeriodoHorarioServico : IBaseServico<PeriodoHorario>
    {
    }

    public class PeriodoHorarioServico : BaseServico<PeriodoHorario, IPeriodoHorarioRepositorio>, IPeriodoHorarioServico
    {
    }
}