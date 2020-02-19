using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;

namespace Repositorio
{
    public class EstoqueRepositorio : NHibRepository<Estoque>, IEstoqueRepositorio
    {
        public EstoqueRepositorio(NHibContext context)
            : base(context)
        {
        }
    }
}