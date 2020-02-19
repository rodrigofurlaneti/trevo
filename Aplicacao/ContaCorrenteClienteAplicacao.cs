using Aplicacao.Base;
using Dominio;
using Entidade;

namespace Aplicacao
{
    public interface IContaCorrenteClienteAplicacao : IBaseAplicacao<ContaCorrenteCliente>
    {
    }

    public class ContaCorrenteClienteAplicacao : BaseAplicacao<ContaCorrenteCliente, IContaCorrenteClienteServico>, IContaCorrenteClienteAplicacao
    {
    }
}