using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;

namespace Repositorio
{
    public class PedidoLocacaoLancamentoCobrancaRepositorio : NHibRepository<PedidoLocacaoLancamentoCobranca>, IPedidoLocacaoLancamentoCobrancaRepositorio
    {
        public PedidoLocacaoLancamentoCobrancaRepositorio(NHibContext context)
            : base(context)
        {
        }
    }
}
