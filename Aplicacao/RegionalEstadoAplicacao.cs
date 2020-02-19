using Aplicacao.Base;
using Dominio;
using Entidade;

namespace Aplicacao
{
    public interface IRegionalEstadoAplicacao : IBaseAplicacao<RegionalEstado>
    {
    }

    public class RegionalEstadoAplicacao : BaseAplicacao<RegionalEstado, IRegionalEstadoServico>, IRegionalEstadoAplicacao
    {
    }
}