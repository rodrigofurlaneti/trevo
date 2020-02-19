using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;

namespace Dominio
{
    public interface IPlanoCarreiraServico : IBaseServico<PlanoCarreira>
    {

    }

    public class PlanoCarreiraServico : BaseServico<PlanoCarreira, IPlanoCarreiraRepositorio>, IPlanoCarreiraServico
    {

    }
}