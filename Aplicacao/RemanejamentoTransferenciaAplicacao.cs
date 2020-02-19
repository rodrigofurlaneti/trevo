using Aplicacao.Base;
using Dominio;
using Entidade;

namespace Aplicacao
{
    public interface IRemanejamentoTransferenciaAplicacao : IBaseAplicacao<RemanejamentoTransferencia>
    {

    }

    public class RemanejamentoTransferenciaAplicacao : BaseAplicacao<RemanejamentoTransferencia, IRemanejamentoTransferenciaServico>, IRemanejamentoTransferenciaAplicacao
    {
    }
}
