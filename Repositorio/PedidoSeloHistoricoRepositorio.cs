using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;

namespace Repositorio
{
    public class PedidoSeloHistoricoRepositorio : NHibRepository<PedidoSeloHistorico>, IPedidoSeloHistoricoRepositorio
    {
        public PedidoSeloHistoricoRepositorio(NHibContext context)
            : base(context)
        {
        }
    }
}