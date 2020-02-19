using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;

namespace Repositorio
{
    public class TipoMensalistaRepositorio : NHibRepository<TipoMensalista>, ITipoMensalistaRepositorio
    {
        public TipoMensalistaRepositorio(NHibContext context)
            : base(context)
        {
        }
    }
}