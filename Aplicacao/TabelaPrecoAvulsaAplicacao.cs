using Aplicacao.Base;
using Dominio;
using Entidade;

namespace Aplicacao
{

    public interface ITabelaPrecoAvulsaAplicacao : IBaseAplicacao<TabelaPrecoAvulsa>
    {
    }

    public class TabelaPrecoAvulsaAplicacao : BaseAplicacao<TabelaPrecoAvulsa, ITabelaPrecoAvulsaServico>, ITabelaPrecoAvulsaAplicacao
    {
    }
}
