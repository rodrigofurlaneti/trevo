using Aplicacao.Base;
using Dominio;
using Entidade;

namespace Aplicacao
{
    public interface IPedidoSeloEmailAplicacao : IBaseAplicacao<PedidoSeloEmail>
    {
    }

    public class PedidoSeloEmailAplicacao : BaseAplicacao<PedidoSeloEmail, IPedidoSeloEmailServico>, IPedidoSeloEmailAplicacao
    {
    }
}