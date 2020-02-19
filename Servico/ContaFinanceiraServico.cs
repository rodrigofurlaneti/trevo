using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;

namespace Dominio
{
    public interface IContaFinanceiraServico : IBaseServico<ContaFinanceira>
    {
    }

    public class ContaFinanceiraServico : BaseServico<ContaFinanceira, IContaFinanceiraRepositorio>, IContaFinanceiraServico
    {
    }
}