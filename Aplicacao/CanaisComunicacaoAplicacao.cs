using Aplicacao.Base;
using Dominio;
using Entidade;

namespace Aplicacao
{
    public interface ICanaisComunicacaoAplicacao : IBaseAplicacao<CanaisComunicacao>
    {
    }

    public class CanaisComunicacaoAplicacao : BaseAplicacao<CanaisComunicacao, ICanaisComunicacaoServico>, ICanaisComunicacaoAplicacao
    {
    }
}