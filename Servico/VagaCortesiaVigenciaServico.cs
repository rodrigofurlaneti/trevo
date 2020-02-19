using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;

namespace Dominio
{
    public interface IVagaCortesiaVigenciaServico : IBaseServico<VagaCortesiaVigencia>
    {
    }

    public class VagaCortesiaVigenciaServico : BaseServico<VagaCortesiaVigencia, IVagaCortesiaVigenciaRepositorio>, IVagaCortesiaVigenciaServico
    {
    }
}