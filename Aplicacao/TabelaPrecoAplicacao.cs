using Aplicacao.Base;
using Dominio;
using Entidade;

namespace Aplicacao
{
    public interface ITabelaPrecoAplicacao : IBaseAplicacao<TabelaPreco>
    {

    }
    public class TabelaPrecoAplicacao : BaseAplicacao<TabelaPreco, ITabelaPrecoServico>, ITabelaPrecoAplicacao
    {
    }
}
