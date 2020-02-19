using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;

namespace Dominio
{

    public interface IConvenioUnidadeServico : IBaseServico<ConvenioUnidade>
    {

    }

    public class ConvenioUnidadeServico : BaseServico<ConvenioUnidade, IConvenioUnidadeRepositorio>, IConvenioUnidadeServico
    {
    }
}
