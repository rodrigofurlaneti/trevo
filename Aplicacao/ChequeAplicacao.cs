using Aplicacao.Base;
using Dominio;
using Entidade;

namespace Aplicacao
{
    public interface IChequeAplicacao : IBaseAplicacao<Cheque>
    {
    }

    public class ChequeAplicacao : BaseAplicacao<Cheque, IChequeServico>, IChequeAplicacao
    {
    }
}