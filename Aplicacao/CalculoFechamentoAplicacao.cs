using Aplicacao.Base;
using Dominio;
using Entidade;

namespace Aplicacao
{
    public interface ICalculoFechamentoAplicacao : IBaseAplicacao<CalculoFechamento>
    {

    }

    public class CalculoFechamentoAplicacao : BaseAplicacao<CalculoFechamento, ICalculoFechamentoServico>, ICalculoFechamentoAplicacao
    {

    }
}
