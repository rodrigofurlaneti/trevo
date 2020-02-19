using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;

namespace Repositorio
{
    public class RemanejamentoRepositorio : NHibRepository<Remanejamento>, IRemanejamentoRepositorio
    {
        public RemanejamentoRepositorio(NHibContext context) : base(context)
        {

        }
    }
}
