using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;

namespace Dominio
{
    public interface IUnidadeCheckListAtividadeServico: IBaseServico<UnidadeCheckListAtividade>
    {

    }

    public class UnidadeCheckListAtividadeServico: BaseServico<UnidadeCheckListAtividade, IUnidadeCheckListAtividadeRepositorio>, IUnidadeCheckListAtividadeServico
    {
    }
}
