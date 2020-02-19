using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;

namespace Dominio
{
    public interface IParametroNotificacaoServico : IBaseServico<ParametroNotificacao>
    {

    }

    public class ParametroNotificacaoServico : BaseServico<ParametroNotificacao, IParametroNotificacaoRepositorio>, IParametroNotificacaoServico
    {
    }
}
