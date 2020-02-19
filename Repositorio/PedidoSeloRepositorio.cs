using Dominio.IRepositorio;
using Entidade;
using Entidade.Uteis;
using Repositorio.Base;
using System.Collections.Generic;
using System.Linq;

namespace Repositorio
{
    public class PedidoSeloRepositorio : NHibRepository<PedidoSelo>, IPedidoSeloRepositorio
    {
        public PedidoSeloRepositorio(NHibContext context) : base(context)
        {   
        }

        public IList<PedidoSelo> ConsultaPedidosPorStatus(StatusPedidoSelo status)
        {
            return Session.GetListBy<PedidoSelo>(x => x.StatusPedido == status).ToList();
        }

        public bool PedidoPassouPeloStatus(int idPedido, StatusPedidoSelo status)
        {
            var pedidoHistorico = Session.GetItemBy<PedidoSeloHistorico>(x => x.PedidoSelo.Id == idPedido && x.StatusPedidoSelo == status);
            return pedidoHistorico != null;
        }
    }
}