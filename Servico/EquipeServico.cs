using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;

namespace Dominio
{
    public interface IEquipeServico : IBaseServico<Equipe>
    {

    }
    public class EquipeServico : BaseServico<Equipe,IEquipeRepositorio>, IEquipeServico
    {

    }
}
