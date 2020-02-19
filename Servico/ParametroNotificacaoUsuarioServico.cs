using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;

namespace Dominio
{
    public interface IParametroNotificacaoUsuarioServico : IBaseServico<ParametroNotificacaoUsuario>
    {

    }

    public class ParametroNotificacaoUsuarioServico : BaseServico<ParametroNotificacaoUsuario, IParametroNotificacaoUsuarioRepositorio>, IParametroNotificacaoUsuarioServico
    {
    }
}
