using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;

namespace Repositorio
{
    public class EstruturaUnidadeRepositorio : NHibRepository<EstruturaUnidade>, IEstruturaUnidadeRepositorio
    {
        public EstruturaUnidadeRepositorio(NHibContext context) : base(context)
        {

        }
    }
}
