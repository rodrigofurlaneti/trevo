using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;

namespace Repositorio
{
    public class EstoqueManualItemRepositorio : NHibRepository<EstoqueManualItem>, IEstoqueManualItemRepositorio
    {
        public EstoqueManualItemRepositorio(NHibContext context) : base(context)
        {

        }
    }
}