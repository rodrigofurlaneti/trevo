using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;

namespace Dominio
{
    public interface IImportacaoPagamentoServico : IBaseServico<ImportacaoPagamento>
    {
    }

    public class ImportacaoPagamentoServico : BaseServico<ImportacaoPagamento, IImportacaoPagamentoRepositorio>, IImportacaoPagamentoServico
    {
    }
}