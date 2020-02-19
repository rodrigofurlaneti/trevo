using Aplicacao.Base;
using Dominio;
using Entidade;

namespace Aplicacao
{
    public interface IContratoUnidadeContasAPagarAplicacao : IBaseAplicacao<ContratoUnidadeContasAPagar>
    {

    }

    public class ContratoUnidadeContasAPagarAplicacao : BaseAplicacao<ContratoUnidadeContasAPagar, IContratoUnidadeContasAPagarServico>, IContratoUnidadeContasAPagarAplicacao
    {
    }
}
