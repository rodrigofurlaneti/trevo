using Aplicacao.Base;
using Dominio;
using Entidade;

namespace Aplicacao
{
    public interface IParametrosLayoutAplicacao : IBaseAplicacao<ParametrosLayout>
    {
    }

    public class ParametrosLayoutAplicacao : BaseAplicacao<ParametrosLayout, IParametrosLayoutServico>, IParametrosLayoutAplicacao
    {
    }
}