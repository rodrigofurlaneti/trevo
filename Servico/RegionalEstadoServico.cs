using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;

namespace Dominio
{
    public interface IRegionalEstadoServico : IBaseServico<RegionalEstado>
    {
    }

    public class RegionalEstadoServico : BaseServico<RegionalEstado, IRegionalEstadoRepositorio>, IRegionalEstadoServico
    {
    }
}