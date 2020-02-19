using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;

namespace Repositorio
{
    public class PecaServicoRepositorio : NHibRepository<PecaServico>, IPecaServicoRepositorio
    {
        public PecaServicoRepositorio(NHibContext context)
            : base(context)
        {
        }
    }
}