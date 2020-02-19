using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;

namespace Dominio
{
    public interface IEnderecoServico : IBaseServico<Endereco>
    {
    }

    public class EnderecoServico : BaseServico<Endereco, IEnderecoRepositorio>, IEnderecoServico
    {
    }
}