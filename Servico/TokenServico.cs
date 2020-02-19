using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;

namespace Dominio
{
    public interface ITokenServico : IBaseServico<Token>
    {
    }

    public class TokenServico : BaseServico<Token, ITokenRepositorio>, ITokenServico
    {
    }
}