using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;
using System.Linq;

namespace Dominio
{
    public interface IContaCorrenteClienteServico : IBaseServico<ContaCorrenteCliente>
    {
    }

    public class ContaCorrenteClienteServico : BaseServico<ContaCorrenteCliente, IContaCorrenteClienteRepositorio>, IContaCorrenteClienteServico
    {

    }
}