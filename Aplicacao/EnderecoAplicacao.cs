using Aplicacao.Base;
using Dominio;
using Entidade;

namespace Aplicacao
{
    public interface IEnderecoAplicacao : IBaseAplicacao<Endereco>
    {
    }

    public class EnderecoAplicacao : BaseAplicacao<Endereco, IEnderecoServico>, IEnderecoAplicacao
    {
    }
}