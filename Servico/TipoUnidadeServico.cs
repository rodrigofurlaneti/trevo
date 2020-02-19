using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;

namespace Dominio
{
    public interface ITipoUnidadeServico: IBaseServico<TipoUnidade>
    {

    }

    public class TipoUnidadeServico : BaseServico<TipoUnidade, ITipoUnidadeRepositorio>, ITipoUnidadeServico
    {
    }
}
