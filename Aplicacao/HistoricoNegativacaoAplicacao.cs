using Aplicacao.Base;
using Dominio;
using Entidade;

namespace Aplicacao
{
    public interface IHistoricoNegativacaoAplicacao : IBaseAplicacao<HistoricoNegativacao>
    {
    }

    public class HistoricoNegativacaoAplicacao : BaseAplicacao<HistoricoNegativacao, IHistoricoNegativacaoServico>, IHistoricoNegativacaoAplicacao
    {
    }
}