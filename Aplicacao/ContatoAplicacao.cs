using Aplicacao.Base;
using Dominio;
using Entidade;

namespace Aplicacao
{
    public interface IContatoAplicacao : IBaseAplicacao<Contato>
    {
    }

    public class ContatoAplicacao : BaseAplicacao<Contato, IContatoServico>, IContatoAplicacao
    {
    }
}