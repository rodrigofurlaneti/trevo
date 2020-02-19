using Aplicacao.Base;
using Dominio;
using Entidade;

namespace Aplicacao
{
    public interface IEquipamentoAplicacao : IBaseAplicacao<Equipamento>
    {

    }

    public class EquipamentoAplicacao : BaseAplicacao<Equipamento, IEquipamentoServico>, IEquipamentoAplicacao
    {
    }
}
