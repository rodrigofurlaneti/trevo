using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;

namespace Dominio
{
    public interface ITipoOcorrenciaServico : IBaseServico<TipoOcorrencia>
    {

    }

    public class TipoOcorrenciaServico : BaseServico<TipoOcorrencia, ITipoOcorrenciaRepositorio>, ITipoOcorrenciaServico
    {

    }
}