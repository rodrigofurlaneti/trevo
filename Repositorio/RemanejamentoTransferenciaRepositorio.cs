using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;

namespace Repositorio
{
    public class RemanejamentoTransferenciaRepositorio : NHibRepository<RemanejamentoTransferencia>, IRemanejamentoTransferenciaRepositorio
    {
        public RemanejamentoTransferenciaRepositorio(NHibContext context) : base(context)
        {

        }
    }
}
