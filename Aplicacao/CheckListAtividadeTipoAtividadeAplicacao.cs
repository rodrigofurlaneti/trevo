using Aplicacao.Base;
using Dominio;
using Entidade;

namespace Aplicacao
{
    public interface ICheckListAtividadeTipoAtividadeAplicacao : IBaseAplicacao<CheckListAtividadeTipoAtividade>
    {

    }

    public class CheckListAtividadeTipoAtividadeAplicacao : BaseAplicacao<CheckListAtividadeTipoAtividade, ICheckListAtividadeTipoAtividadeServico>, ICheckListAtividadeTipoAtividadeAplicacao
    {
    }
}
