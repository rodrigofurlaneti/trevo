using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;

namespace Dominio
{
    public interface ITipoFilialServico : IBaseServico<TipoFilial>
    {
    }

    public class TipoFilialServico : BaseServico<TipoFilial, ITipoFilialRepositorio>, ITipoFilialServico
    {
    }
}
