using Core.Exceptions;
using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;
using Entidade.Uteis;

namespace Dominio
{
    public interface ITipoNotificacaoServico : IBaseServico<TipoNotificacao>
    {
    }

    public class TipoNotificacaoServico : BaseServico<TipoNotificacao, ITipoNotificacaoRepositorio>, ITipoNotificacaoServico
    {
    }
}