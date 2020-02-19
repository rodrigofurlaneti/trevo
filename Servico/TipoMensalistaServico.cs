using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;

namespace Dominio
{
    public interface ITipoMensalistaServico : IBaseServico<TipoMensalista>
    {
    }

    public class TipoMensalistaServico : BaseServico<TipoMensalista, ITipoMensalistaRepositorio>, ITipoMensalistaServico
    {
    }
}