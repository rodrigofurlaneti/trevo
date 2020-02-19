using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;

namespace Repositorio
{
    public class PaisRepositorio : NHibRepository<Pais>, IPaisRepositorio
    {
        public PaisRepositorio(NHibContext context)
            : base(context)
        {
        }
    }
}