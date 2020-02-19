using System.Linq;
using Aplicacao.Base;
using Dominio;
using Entidade;
using Entidade.Uteis;

namespace Aplicacao
{
    public interface IOcorrenciaFuncionarioAplicacao : IBaseAplicacao<OcorrenciaFuncionario>
    {
    }

    public class OcorrenciaFuncionarioAplicacao : BaseAplicacao<OcorrenciaFuncionario, IOcorrenciaFuncionarioServico>, IOcorrenciaFuncionarioAplicacao
    {
    }
}