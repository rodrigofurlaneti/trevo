using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;

namespace Repositorio
{
    public class EstruturaGaragemRepositorio : NHibRepository<EstruturaGaragem>, IEstruturaGaragemRepositorio
    {
        public EstruturaGaragemRepositorio(NHibContext context)
            : base(context)
        {
        }
    }
}