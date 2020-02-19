using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;

namespace Dominio
{
    public interface ICheckListAtividadeTipoAtividadeServico : IBaseServico<CheckListAtividadeTipoAtividade>
    {

    }

    public class CheckListAtividadeTipoAtividadeServico : BaseServico<CheckListAtividadeTipoAtividade, ICheckListAtividadeTipoAtividadeRepositorio>, ICheckListAtividadeTipoAtividadeServico
    {
    }
}
