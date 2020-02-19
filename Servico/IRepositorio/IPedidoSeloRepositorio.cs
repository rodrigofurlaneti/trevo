using Dominio.IRepositorio.Base;
using Entidade;
using Entidade.Uteis;
using System.Collections.Generic;

namespace Dominio.IRepositorio
{
    public interface IPedidoSeloRepositorio : IRepository<PedidoSelo>
    {
        IList<PedidoSelo> ConsultaPedidosPorStatus(StatusPedidoSelo status);
        bool PedidoPassouPeloStatus(int idPedido, StatusPedidoSelo status);
    }
}