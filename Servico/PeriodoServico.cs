using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;

namespace Dominio
{
    public interface IPeriodoServico : IBaseServico<Periodo>
    {

    }
    public class PeriodoServico : BaseServico<Periodo, IPeriodoRepositorio>, IPeriodoServico
    {
    }
}
