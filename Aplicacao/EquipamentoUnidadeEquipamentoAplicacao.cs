using Aplicacao.Base;
using Dominio;
using Entidade;

namespace Aplicacao
{
    public interface IEquipamentoUnidadeEquipamentoAplicacao : IBaseAplicacao<EquipamentoUnidadeEquipamento>
    {

    }

    public class EquipamentoUnidadeEquipamentoAplicacao : BaseAplicacao<EquipamentoUnidadeEquipamento, IEquipamentoUnidadeEquipamentoServico>, IEquipamentoUnidadeEquipamentoAplicacao
    {
    }
}
