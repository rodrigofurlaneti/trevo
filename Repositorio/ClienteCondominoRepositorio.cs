using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;

namespace Repositorio
{
    public class ClienteCondominoRepositorio : NHibRepository<ClienteCondomino>, IClienteCondominoRepositorio
    {
        public ClienteCondominoRepositorio(NHibContext context)
            : base(context)
        {
        }
    }
}