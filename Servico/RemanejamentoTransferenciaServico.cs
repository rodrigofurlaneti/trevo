using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;

namespace Dominio
{
    public interface IRemanejamentoTransferenciaServico : IBaseServico<RemanejamentoTransferencia>
    {

    }
    public class RemanejamentoTransferenciaServico : BaseServico<RemanejamentoTransferencia, IRemanejamentoTransferenciaRepositorio>, IRemanejamentoTransferenciaServico
    {
    }
}
