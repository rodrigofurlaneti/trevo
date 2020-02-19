using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;

namespace Repositorio
{
    public class EmpresaRepositorio : NHibRepository<Empresa>, IEmpresaRepositorio
    {
        public EmpresaRepositorio(NHibContext context) : base(context)
        {
        }
    }
}