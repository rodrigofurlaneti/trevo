using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;

namespace Repositorio
{
    public class ControleFeriasRepositorio : NHibRepository<ControleFerias>, IControleFeriasRepositorio
    {
        public ControleFeriasRepositorio(NHibContext context)
            : base(context)
        {

        }
    }
}