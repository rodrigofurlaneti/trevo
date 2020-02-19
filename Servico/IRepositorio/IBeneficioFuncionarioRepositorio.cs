using Dominio.IRepositorio.Base;
using Entidade;

namespace Dominio.IRepositorio
{
    public interface IBeneficioFuncionarioRepositorio : IRepository<BeneficioFuncionario>
    {
        void DeleteOrphan();
    }
}