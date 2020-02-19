using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;

namespace Dominio
{
    public interface ITipoPagamentoServico : IBaseServico<TipoPagamento>
    {
         
    }
    public  class TipoPagamentoServico : BaseServico<TipoPagamento,ITipoPagamentoRepositorio>, ITipoPagamentoServico
    {
    }
}
