using Aplicacao.Base;
using Dominio;
using Entidade;

namespace Aplicacao
{
    public interface IOficinaAplicacao : IBaseAplicacao<Oficina>
    {
    }

    public class OficinaAplicacao : BaseAplicacao<Oficina, IOficinaServico>, IOficinaAplicacao
    {
        
    }
}