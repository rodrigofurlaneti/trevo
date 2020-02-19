using Aplicacao.Base;
using Dominio;
using Entidade;

namespace Aplicacao
{
    public interface IHorarioPrecoAplicacao : IBaseAplicacao<HorarioPreco>
    {

    }
    public class HorarioPrecoAplicacao : BaseAplicacao<HorarioPreco,IHorarioPrecoServico>, IHorarioPrecoAplicacao
    {
    }
}
