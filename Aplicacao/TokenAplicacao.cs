using Aplicacao.Base;
using Dominio;
using Entidade;

namespace Aplicacao
{
    public interface ITokenAplicacao : IBaseAplicacao<Token>
    {
    }

    public class TokenAplicacao : BaseAplicacao<Token, ITokenServico>, ITokenAplicacao
    {
    }
}