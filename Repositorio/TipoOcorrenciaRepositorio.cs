using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;

namespace Repositorio
{
    public class TipoOcorrenciaRepositorio : NHibRepository<TipoOcorrencia>, ITipoOcorrenciaRepositorio
    {
        public TipoOcorrenciaRepositorio(NHibContext context)
            : base(context)
        {

        }
    }
}