using Dominio.IRepositorio;
using Entidade.Base;
using Repositorio.Base;

namespace Repositorio
{
    public class AuditRepositorio : NHibRepository<Audit>, IAuditRepositorio
    {
        public AuditRepositorio(NHibContext context)
            : base(context)
        {
        }
    }
}