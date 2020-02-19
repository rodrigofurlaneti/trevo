using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;

namespace Repositorio
{
    public class DepartamentoRepositorio : NHibRepository<Departamento>, IDepartamentoRepositorio
    {
        public DepartamentoRepositorio(NHibContext context)
            : base(context)
        {
        }
    }
}