using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;

namespace Dominio
{
    public interface IEquipamentoServico : IBaseServico<Equipamento>
    {

    }

    public class EquipamentoServico : BaseServico<Equipamento, IEquipamentoRepositorio>, IEquipamentoServico
    {
    }
}
