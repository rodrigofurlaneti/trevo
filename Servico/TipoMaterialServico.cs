using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;

namespace Dominio
{
    public interface ITipoMaterialServico : IBaseServico<TipoMaterial>
    {
    }

    public class TipoMaterialServico : BaseServico<TipoMaterial, ITipoMaterialRepositorio>, ITipoMaterialServico
    {
        
    }
}