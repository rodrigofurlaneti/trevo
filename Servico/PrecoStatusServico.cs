using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;

namespace Dominio
{
    public interface IPrecoStatusServico : IBaseServico<PrecoStatus>
    {

    }

    public class PrecoStatusServico : BaseServico<PrecoStatus, IPrecoStatusRepositorio>, IPrecoStatusServico
    {
    }
}
