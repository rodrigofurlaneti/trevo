using Aplicacao.Base;
using Dominio;
using Entidade;

namespace Aplicacao
{
    public interface IRemanejamentoAplicacao : IBaseAplicacao<Remanejamento>
    {

    }

    public class RemanejamentoAplicacao : BaseAplicacao<Remanejamento, IRemanejamentoServico>, IRemanejamentoAplicacao
    {

    }
}
