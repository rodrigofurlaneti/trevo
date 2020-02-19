using Aplicacao.Base;
using Dominio;
using Entidade;

namespace Aplicacao
{
    public interface IPaisAplicacao : IBaseAplicacao<Pais>
    {
    }

    public class PaisAplicacao : BaseAplicacao<Pais, IPaisServico>, IPaisAplicacao
    {
    }
}