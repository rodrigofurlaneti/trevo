using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;

namespace Repositorio
{
    public class PedidoSeloEmailRepositorio : NHibRepository<PedidoSeloEmail>, IPedidoSeloEmailRepositorio
    {
        public PedidoSeloEmailRepositorio(NHibContext context)
            : base(context)
        {
        }
    }
}