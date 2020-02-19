using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;

namespace Repositorio
{
    public class VagaCortesiaVigenciaRepositorio : NHibRepository<VagaCortesiaVigencia>, IVagaCortesiaVigenciaRepositorio
    {
        public VagaCortesiaVigenciaRepositorio(NHibContext context)
            : base(context)
        {
        }
    }
}