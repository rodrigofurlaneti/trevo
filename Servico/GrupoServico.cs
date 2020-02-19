using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;

namespace Dominio
{
    public interface IGrupoServico : IBaseServico<Grupo>
    {
    }

    public class GrupoServico : BaseServico<Grupo, IGrupoRepositorio>, IGrupoServico
    {
    }
}