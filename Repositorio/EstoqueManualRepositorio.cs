using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;

namespace Repositorio
{
    public class EstoqueManualRepositorio : NHibRepository<EstoqueManual>, IEstoqueManualRepositorio
    {
        public EstoqueManualRepositorio(NHibContext context)
            : base(context)
        {
        }
    }
}