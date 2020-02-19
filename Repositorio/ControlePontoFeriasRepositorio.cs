using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;

namespace Repositorio
{
    public class ControlePontoFeriasRepositorio : NHibRepository<ControlePontoFerias>, IControlePontoFeriasRepositorio
    {
        public ControlePontoFeriasRepositorio(NHibContext context)
            : base(context)
        {

        }
    }
}