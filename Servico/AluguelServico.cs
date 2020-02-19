using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;

namespace Dominio
{
    public interface IAluguelServico : IBaseServico<Aluguel>
    {

    }
    public class AluguelServico : BaseServico<Aluguel,IAluguelRepositorio>, IAluguelServico
    {
    }
}
