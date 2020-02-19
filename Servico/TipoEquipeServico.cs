using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;

namespace Dominio
{
    public interface ITipoEquipeServico : IBaseServico<TipoEquipe>
    {
    }

    public class TipoEquipeServico : BaseServico<TipoEquipe, ITipoEquipeRepositorio>, ITipoEquipeServico
    {
    }
}