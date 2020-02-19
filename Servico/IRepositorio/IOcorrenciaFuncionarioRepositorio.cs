using Dominio.IRepositorio.Base;
using Entidade;

namespace Dominio.IRepositorio
{
    public interface IOcorrenciaFuncionarioRepositorio : IRepository<OcorrenciaFuncionario>
    {
        void DeleteOrphan();
    }
}