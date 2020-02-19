using Aplicacao.Base;
using Dominio;
using Entidade;

namespace Aplicacao
{
    public interface IPagamentoAplicacao : IBaseAplicacao<Pagamento>
    {
    }

    public class PagamentoAplicacao : BaseAplicacao<Pagamento, IPagamentoServico>, IPagamentoAplicacao
    {
    }
}