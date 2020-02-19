
using Aplicacao.Base;
using Dominio;
using Entidade;

namespace Aplicacao
{
    public interface IParametroNumeroNotaFiscalAplicacao : IBaseAplicacao<ParametroNumeroNotaFiscal>
    {
    }

    public class ParametroNumeroNotaFiscalAplicacao : BaseAplicacao<ParametroNumeroNotaFiscal, IParametroNumeroNotaFiscalServico>, IParametroNumeroNotaFiscalAplicacao
    {
    }
}