using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;

namespace Repositorio
{
    public class ModeloRepositorio : NHibRepository<Modelo>, IModeloRepositorio
    {
        public ModeloRepositorio(NHibContext context)
            : base(context)
        {

        }
    }
}