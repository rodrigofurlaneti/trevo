using Aplicacao.Base;
using Dominio;
using Entidade;

namespace Aplicacao
{
    public interface ITabelaPrecoMensalAplicacao : IBaseAplicacao<TabelaPrecoMensal>
    {

    }

    public class TabelaPrecoMensalAplicacao : BaseAplicacao<TabelaPrecoMensal, ITabelaPrecoMensalServico>, ITabelaPrecoMensalAplicacao
    {
    }
}
