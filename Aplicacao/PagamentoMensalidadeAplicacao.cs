using Aplicacao.Base;
using Dominio;
using Entidade;

namespace Aplicacao
{
    public interface IPagamentoMensalidadeAplicacao : IBaseAplicacao<PagamentoMensalidade>
    {

    }

    public class PagamentoMensalidadeAplicacao : BaseAplicacao<PagamentoMensalidade,IPagamentoMensalidadeServico>, IPagamentoMensalidadeAplicacao
    {
    }
}
