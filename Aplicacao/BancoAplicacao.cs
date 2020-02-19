using Aplicacao.Base;
using Dominio;
using Entidade;

namespace Aplicacao
{
    public interface IBancoAplicacao : IBaseAplicacao<Banco>
    {
    }

    public class BancoAplicacao : BaseAplicacao<Banco, IBancoServico>, IBancoAplicacao
    {
    }
}