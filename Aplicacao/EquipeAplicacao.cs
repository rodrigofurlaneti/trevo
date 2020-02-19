using Aplicacao.Base;
using Dominio;
using Entidade;

namespace Aplicacao
{
    public interface IEquipeAplicacao : IBaseAplicacao<Equipe>
    {

    }
    public class EquipeAplicacao : BaseAplicacao<Equipe, IEquipeServico>, IEquipeAplicacao
    {

    }
}
