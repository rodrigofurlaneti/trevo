using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;

namespace Repositorio
{
    public class MarcaRepositorio : NHibRepository<Marca>, IMarcaRepositorio
    {
        public MarcaRepositorio(NHibContext context)
            : base(context)
        {
        }
    }
}