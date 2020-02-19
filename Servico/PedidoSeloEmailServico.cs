using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;

namespace Dominio
{
    public interface IPedidoSeloEmailServico : IBaseServico<PedidoSeloEmail>
    {
    }

    public class PedidoSeloEmailServico : BaseServico<PedidoSeloEmail, IPedidoSeloEmailRepositorio>, IPedidoSeloEmailServico
    {
    }
}