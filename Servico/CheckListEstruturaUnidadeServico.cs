using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;

namespace Dominio
{
    public interface ICheckListEstruturaUnidadeServico : IBaseServico<CheckListEstruturaUnidade>
    {
    }

    public class CheckListEstruturaUnidadeServico : BaseServico<CheckListEstruturaUnidade, ICheckListEstruturaUnidadeRepositorio>, ICheckListEstruturaUnidadeServico
    {
    }
}