using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;

namespace Dominio
{

    public interface IChequeEmitidoContaPagarServico : IBaseServico<ChequeEmitidoContaPagar>
    {

    }

    public class ChequeEmitidoContaPagarServico : BaseServico<ChequeEmitidoContaPagar,IChequeEmitidoContaPagarRepositorio>, IChequeEmitidoContaPagarServico
    {
    }
}
