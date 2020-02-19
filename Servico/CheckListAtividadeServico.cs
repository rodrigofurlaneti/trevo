using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;

namespace Dominio
{
    public interface ICheckListAtividadeServico : IBaseServico<CheckListAtividade>
    {
    }

    public class CheckListAtividadeServico : BaseServico<CheckListAtividade, ICheckListAtividadeRepositorio>, ICheckListAtividadeServico
    {
    }
}