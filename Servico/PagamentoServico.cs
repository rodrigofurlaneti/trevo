using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;

namespace Dominio
{
    public interface IPagamentoServico : IBaseServico<Pagamento>
    {
    }

    public class PagamentoServico : BaseServico<Pagamento, IPagamentoRepositorio>, IPagamentoServico
    {
    }
}