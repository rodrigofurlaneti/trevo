using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;

namespace Repositorio
{
    public class OcorrenciaRetornoCNABRepositorio : NHibRepository<OcorrenciaRetornoCNAB>, IOcorrenciaRetornoCNABRepositorio
    {
        public OcorrenciaRetornoCNABRepositorio(NHibContext context)
            : base(context)
        {
        }
    }
}