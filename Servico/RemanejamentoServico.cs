using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;

namespace Dominio
{
    public interface IRemanejamentoServico : IBaseServico<Remanejamento>
    {

    }

    public class RemanejamentoServico : BaseServico<Remanejamento, IRemanejamentoRepositorio>, IRemanejamentoServico
    {

    }
}
