using Aplicacao.Base;
using Dominio;
using Entidade;

namespace Aplicacao
{
    public interface IBeneficioFuncionarioAplicacao : IBaseAplicacao<BeneficioFuncionario>
    {
    }

    public class BeneficioFuncionarioAplicacao : BaseAplicacao<BeneficioFuncionario, IBeneficioFuncionarioServico>, IBeneficioFuncionarioAplicacao
    {
    }
}