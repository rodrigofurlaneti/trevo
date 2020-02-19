using Aplicacao.Base;
using Dominio;
using Entidade;

namespace Aplicacao
{
    public interface IParametroBoletoBancarioAplicacao : IBaseAplicacao<ParametroBoletoBancario>
    {
    }

    public class ParametroBoletoBancarioAplicacao : BaseAplicacao<ParametroBoletoBancario, IParametroBoletoBancarioServico>, IParametroBoletoBancarioAplicacao
    {
    }
}