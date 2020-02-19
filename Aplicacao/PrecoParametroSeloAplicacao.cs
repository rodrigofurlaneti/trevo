using Aplicacao.Base;
using Dominio;
using Entidade;

namespace Aplicacao
{
    public interface IPrecoParametroSeloAplicacao : IBaseAplicacao<PrecoParametroSelo>
    {

    }
    public class PrecoParametroSeloAplicacao : BaseAplicacao<PrecoParametroSelo, IPrecoParametroSeloServico>, IPrecoParametroSeloAplicacao
    {
    }
}