using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;

namespace Repositorio
{
    public class VagaCortesiaRepositorio : NHibRepository<VagaCortesia>, IVagaCortesiaRepositorio
    {
        public VagaCortesiaRepositorio(NHibContext context) : base(context)
        {

        }
    }
}
