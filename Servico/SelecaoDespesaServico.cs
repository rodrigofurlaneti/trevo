using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;

namespace Dominio
{
    public interface ISelecaoDespesaServico : IBaseServico<SelecaoDespesa>
    {
        void RemoverPorContaAPagarId(int id);
    }
    public class SelecaoDespesaServico : BaseServico<SelecaoDespesa, ISelecaoDespesaRepositorio>, ISelecaoDespesaServico
    {
        public void RemoverPorContaAPagarId(int id)
        {
            
        }
    }
}
