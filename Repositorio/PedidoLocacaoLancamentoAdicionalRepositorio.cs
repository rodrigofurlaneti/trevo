using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;

namespace Repositorio
{
    public class PedidoLocacaoLancamentoAdicionalRepositorio : NHibRepository<PedidoLocacaoLancamentoAdicional>, IPedidoLocacaoLancamentoAdicionalRepositorio
    {
        public PedidoLocacaoLancamentoAdicionalRepositorio(NHibContext context) : base(context)
        {   
        }
    }
}