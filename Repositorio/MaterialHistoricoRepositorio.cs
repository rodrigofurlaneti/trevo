using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;

namespace Repositorio
{
    public class MaterialHistoricoRepositorio : NHibRepository<MaterialHistorico>, IMaterialHistoricoRepositorio
    {
        public MaterialHistoricoRepositorio(NHibContext context)
            : base(context)
        {
        }
    }
}