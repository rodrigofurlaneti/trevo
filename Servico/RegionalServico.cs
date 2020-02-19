using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;

namespace Dominio
{
    public interface IRegionalServico : IBaseServico<Regional>
    {
    }

    public class RegionalServico : BaseServico<Regional, IRegionalRepositorio>, IRegionalServico
    {
    }
}