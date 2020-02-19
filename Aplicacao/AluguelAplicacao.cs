using Aplicacao.Base;
using Dominio;
using Entidade;

namespace Aplicacao
{
    public interface IAluguelAplicacao : IBaseAplicacao<Aluguel>
    {

    }
    public class AluguelAplicacao : BaseAplicacao<Aluguel, IAluguelServico>, IAluguelAplicacao
    {
    }
}
