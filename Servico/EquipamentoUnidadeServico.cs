using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;

namespace Dominio
{
    public interface IEquipamentoUnidadeServico : IBaseServico<EquipamentoUnidade>
    {

    }
    public class EquipamentoUnidadeServico : BaseServico<EquipamentoUnidade, IEquipamentoUnidadeRepositorio>, IEquipamentoUnidadeServico
    {
    }
}
