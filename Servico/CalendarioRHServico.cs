using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;

namespace Dominio
{
    public interface ICalendarioRHServico : IBaseServico<CalendarioRH>
    {

    }

    public class CalendarioRHServico : BaseServico<CalendarioRH, ICalendarioRHRepositorio>, ICalendarioRHServico
    {

    }
}