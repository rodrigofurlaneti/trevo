using System.Linq;
using Aplicacao.Base;
using Dominio;
using Entidade;
using Entidade.Uteis;

namespace Aplicacao
{
    public interface IPrecoStatusAplicacao : IBaseAplicacao<PrecoStatus>
    {

    }
    public class PrecoStatusAplicacao : BaseAplicacao<PrecoStatus, IPrecoStatusServico>, IPrecoStatusAplicacao
    {
    }
}
