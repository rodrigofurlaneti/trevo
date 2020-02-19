using Dominio.IRepositorio;
using Entidade;
using Entidade.Uteis;
using Repositorio.Base;
using System.Linq;

namespace Repositorio
{
    public class LancamentoCobrancaPedidoSeloRepositorio : NHibRepository<LancamentoCobrancaPedidoSelo>, ILancamentoCobrancaPedidoSeloRepositorio
    {
        public LancamentoCobrancaPedidoSeloRepositorio(NHibContext context)
            : base(context)
        {
        }

        public LancamentoCobrancaPedidoSelo RetornaUltimoLancamentoCobrancaPorPedidoSelo(int idPedido)
        {
            return Session.GetListBy<LancamentoCobrancaPedidoSelo>(x => x.PedidoSelo.Id == idPedido)
                ?.OrderByDescending(x => x.DataInsercao)
                ?.FirstOrDefault();
        }

        public StatusLancamentoCobranca RetornaStatusPorPedidoSelo(int idPedidoSelo)
        {
            var algumPago = Session.GetListBy<LancamentoCobrancaPedidoSelo>(x => x.PedidoSelo.Id == idPedidoSelo && x.StatusLancamentoCobranca == StatusLancamentoCobranca.Pago).Any();
            if (algumPago) return StatusLancamentoCobranca.Pago;

            return RetornaUltimoLancamentoCobrancaPorPedidoSelo(idPedidoSelo)?.StatusLancamentoCobranca ?? StatusLancamentoCobranca.Novo;
        }
    }
}