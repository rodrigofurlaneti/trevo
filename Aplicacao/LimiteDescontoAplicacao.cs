using Aplicacao.Base;
using Dominio;
using Entidade;

namespace Aplicacao
{
    public interface ILimiteDescontoAplicacao : IBaseAplicacao<LimiteDesconto>
    {

    }

    public class LimiteDescontoAplicacao : BaseAplicacao<LimiteDesconto, ILimiteDescontoServico>, ILimiteDescontoAplicacao
    {
    }
}
