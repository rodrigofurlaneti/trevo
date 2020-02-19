using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;

namespace Dominio
{
    public interface IChequeServico : IBaseServico<Cheque>
    {
    }

    public class ChequeServico : BaseServico<Cheque, IChequeRepositorio>, IChequeServico
    {
    }
}