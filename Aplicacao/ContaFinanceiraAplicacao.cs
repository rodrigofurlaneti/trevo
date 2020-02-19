using Aplicacao.Base;
using Dominio;
using Entidade;

namespace Aplicacao
{
    public interface IContaFinanceiraAplicacao : IBaseAplicacao<ContaFinanceira>
    {
    }

    public class ContaFinanceiraAplicacao : BaseAplicacao<ContaFinanceira, IContaFinanceiraServico>, IContaFinanceiraAplicacao
    {
    }
}