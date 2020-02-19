using Aplicacao.Base;
using Dominio;
using Entidade;

namespace Aplicacao
{
    public interface IMaquinaCartaoAplicacao : IBaseAplicacao<MaquinaCartao>
    {

    }

    public class MaquinaCartaoAplicacao : BaseAplicacao<MaquinaCartao,IMaquinaCartaoServico>, IMaquinaCartaoAplicacao
    {
    }
}
