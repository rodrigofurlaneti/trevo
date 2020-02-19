using Aplicacao.Base;
using Dominio;
using Entidade;

namespace Aplicacao
{
    public interface IEquipamentoUnidadeAplicacao : IBaseAplicacao<EquipamentoUnidade>
    {

    }
    public class EquipamentoUnidadeAplicacao : BaseAplicacao<EquipamentoUnidade, IEquipamentoUnidadeServico>, IEquipamentoUnidadeAplicacao
    {
    }
}
