using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;

namespace Dominio
{
    public interface IEquipamentoUnidadeEquipamentoServico : IBaseServico<EquipamentoUnidadeEquipamento>
    {

    }

    public class EquipamentoUnidadeEquipamentoServico : BaseServico<EquipamentoUnidadeEquipamento, IEquipamentoUnidadeEquipamentoRepositorio>,IEquipamentoUnidadeEquipamentoServico
    {
    }
}
