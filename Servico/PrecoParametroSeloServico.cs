using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;

namespace Dominio
{
    public interface IPrecoParametroSeloServico : IBaseServico<PrecoParametroSelo>
    {

    }

    public class PrecoParametroSeloServico : BaseServico<PrecoParametroSelo, IPrecoParametroSeloRepositorio>, IPrecoParametroSeloServico
    {

    }
}

