using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;

namespace Repositorio
{
    public class ControleCompraRepositorio : NHibRepository<ControleCompra>, IControleCompraRepositorio
    {
        public ControleCompraRepositorio(NHibContext context)
            : base(context)
        {
        }
    }
}