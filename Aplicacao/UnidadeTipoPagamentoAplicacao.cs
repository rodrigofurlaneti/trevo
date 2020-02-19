using Aplicacao.Base;
using Dominio;
using Entidade;

namespace Aplicacao
{
    public interface IUnidadeTipoPagamentoAplicacao: IBaseAplicacao<Tipospagamentos>
    {

    }

    public class UnidadeTipoPagamentoAplicacao : BaseAplicacao<Tipospagamentos,IUnidadeTipoPagamentoServico>, IUnidadeTipoPagamentoAplicacao
    {
    }
}
