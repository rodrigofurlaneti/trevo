using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;

namespace Dominio
{
    public interface ITrabalhoServico : IBaseServico<Trabalho>
    {
    }

    public class TrabalhoServico : BaseServico<Trabalho, ITrabalhoRepositorio>, ITrabalhoServico
    {
    }
}
