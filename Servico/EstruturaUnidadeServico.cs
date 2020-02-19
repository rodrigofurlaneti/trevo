using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;

namespace Dominio
{
    public interface IEstruturaUnidadeServico : IBaseServico<EstruturaUnidade>
    {

    }
    public class EstruturaUnidadeServico : BaseServico<EstruturaUnidade, IEstruturaUnidadeRepositorio>, IEstruturaUnidadeServico
    {
    }
}
