using Aplicacao.Base;
using Dominio;
using Entidade;

namespace Aplicacao
{
    public interface IUnidadeCheckListAtividadeAplicacao : IBaseAplicacao<UnidadeCheckListAtividade>
    {

    }

    public class UnidadeCheckListAtividadeAplicacao : BaseAplicacao<UnidadeCheckListAtividade, UnidadeCheckListAtividadeServico>, IUnidadeCheckListAtividadeAplicacao
    {
    }
}
