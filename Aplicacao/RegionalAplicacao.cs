using Aplicacao.Base;
using Dominio;
using Entidade;

namespace Aplicacao
{
    public interface IRegionalAplicacao : IBaseAplicacao<Regional>
    {
    }

    public class RegionalAplicacao : BaseAplicacao<Regional, IRegionalServico>, IRegionalAplicacao
    {
    }
}