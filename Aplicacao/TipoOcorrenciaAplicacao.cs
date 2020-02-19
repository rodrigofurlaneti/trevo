using System.Linq;
using Aplicacao.Base;
using Dominio;
using Entidade;
using Entidade.Uteis;

namespace Aplicacao
{
    public interface ITipoOcorrenciaAplicacao : IBaseAplicacao<TipoOcorrencia>
    {
    }

    public class TipoOcorrenciaAplicacao : BaseAplicacao<TipoOcorrencia, ITipoOcorrenciaServico>, ITipoOcorrenciaAplicacao
    {
    }
}