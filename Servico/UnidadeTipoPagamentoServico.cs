using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;

namespace Dominio
{
    public interface IUnidadeTipoPagamentoServico : IBaseServico<Tipospagamentos>
    {

    }
    public class UnidadeTipoPagamentoServico : BaseServico<Tipospagamentos,IUnidadeTipoPagamentoRepositorio>, IUnidadeTipoPagamentoServico
    {
    }
}
