using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;

namespace Repositorio
{
    public class NecessidadeCompraRepositorio : NHibRepository<NecessidadeCompra>, INecessidadeCompraRepositorio
    {
        public NecessidadeCompraRepositorio(NHibContext context)
            : base(context)
        {
        }
    }
}