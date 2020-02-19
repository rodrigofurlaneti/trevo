using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;

namespace Dominio
{
    public interface IClienteCondominoServico : IBaseServico<ClienteCondomino>
    {
    }

    public class ClienteCondominoServico : BaseServico<ClienteCondomino, IClienteCondominoRepositorio>, IClienteCondominoServico
    {
    }
}