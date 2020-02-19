using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;

namespace Dominio
{
    public interface IContratoUnidadeServico : IBaseServico<ContratoUnidade>
    {
    }

    public class ContratoUnidadeServico : BaseServico<ContratoUnidade, IContratoUnidadeRepositorio>, IContratoUnidadeServico
    {
    }
}