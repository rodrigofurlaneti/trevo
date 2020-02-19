using Aplicacao.Base;
using Dominio;
using Entidade;

namespace Aplicacao
{
    public interface IImportacaoPagamentoAplicacao : IBaseAplicacao<ImportacaoPagamento>
    {
    }

    public class ImportacaoPagamentoAplicacao : BaseAplicacao<ImportacaoPagamento, IImportacaoPagamentoServico>, IImportacaoPagamentoAplicacao
    {
    }
}