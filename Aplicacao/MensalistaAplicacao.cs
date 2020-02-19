using Aplicacao.Base;
using Dominio;
using Entidade;

namespace Aplicacao
{
    public interface IMensalistaAplicacao : IBaseAplicacao<Mensalista>
    {

    }
    public class MensalistaAplicacao : BaseAplicacao<Mensalista,IMensalistaServico>, IMensalistaAplicacao
    {

    }
}
