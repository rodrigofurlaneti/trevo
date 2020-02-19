using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;

namespace Dominio
{
    public interface IPermissaoServico : IBaseServico<Permissao>
    {
    }

    public class PermissaoServico : BaseServico<Permissao, IPermissaoRepositorio>, IPermissaoServico
    {
    }
}