using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;

namespace Repositorio
{
    public class ControlePontoRepositorio : NHibRepository<ControlePonto>, IControlePontoRepositorio
    {
        public ControlePontoRepositorio(NHibContext context)
            : base(context)
        {

        }
    }
}