using Aplicacao.Base;
using Dominio;
using Entidade;

namespace Aplicacao
{
    public interface ITipoPagamentoAplicacao : IBaseAplicacao<TipoPagamento>
    {

    }
    public class TipoPagamentoAplicacao : BaseAplicacao<TipoPagamento, TipoPagamentoServico>, ITipoPagamentoAplicacao
    {
    }
}
