using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;

namespace Dominio
{
    public interface IContratoUnidadeContasAPagarServico : IBaseServico<ContratoUnidadeContasAPagar>
    {

    }

    public class ContratoUnidadeContasAPagarServico : BaseServico<ContratoUnidadeContasAPagar, IContratoUnidadeContasAPagarRepositorio>, IContratoUnidadeContasAPagarServico
    {
    }
}
