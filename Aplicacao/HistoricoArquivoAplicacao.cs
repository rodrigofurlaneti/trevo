using Aplicacao.Base;
using Dominio;
using Entidade;

namespace Aplicacao
{
    public interface IHistoricoArquivoAplicacao : IBaseAplicacao<HistoricoArquivo>
    {
    }

    public class HistoricoArquivoAplicacao : BaseAplicacao<HistoricoArquivo, IHistoricoArquivoServico>, IHistoricoArquivoAplicacao
    {
    }
}