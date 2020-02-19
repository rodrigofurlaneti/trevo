using Aplicacao.Base;
using Dominio;
using Entidade;

namespace Aplicacao
{
    public interface IEstadoAplicacao : IBaseAplicacao<Estado>
    {
    }

    public class EstadoAplicacao : BaseAplicacao<Estado, IEstadoServico>, IEstadoAplicacao
    {
    }
}