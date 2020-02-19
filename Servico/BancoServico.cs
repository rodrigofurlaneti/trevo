using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;

namespace Dominio
{
    public interface IBancoServico : IBaseServico<Banco>
    {
    }

    public class BancoServico : BaseServico<Banco, IBancoRepositorio>, IBancoServico
    {
    }
}