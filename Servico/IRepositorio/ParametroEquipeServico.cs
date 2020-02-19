using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;

namespace Dominio
{
    public interface IParametroEquipeServico : IBaseServico<ParametroEquipe>
    {

    }
    public class ParametroEquipeServico : BaseServico<ParametroEquipe, IParametroEquipeRepositorio>, IParametroEquipeServico
    {

    }
}
