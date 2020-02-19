using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;

namespace Repositorio
{
    public class TabelaPrecoMensalistaRepositorio : NHibRepository<TabelaPrecoMensalista>, ITabelaPrecoMensalistaRepositorio
    {
        public TabelaPrecoMensalistaRepositorio(NHibContext context) : base(context)
        {

        }
    }
}
