using Aplicacao.Base;
using Dominio;
using Entidade;

namespace Aplicacao
{
    public interface ITipoBeneficioAplicacao : IBaseAplicacao<TipoBeneficio>
    {
    }

    public class TipoBeneficioAplicacao : BaseAplicacao<TipoBeneficio, ITipoBeneficioServico>, ITipoBeneficioAplicacao
    {
    }
}