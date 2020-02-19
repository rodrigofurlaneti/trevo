using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;

namespace Dominio
{
    public interface IModeloServico : IBaseServico<Modelo>
    {
        
    }

    public class ModeloServico : BaseServico<Modelo, IModeloRepositorio>, IModeloServico
    {
        
    }
}