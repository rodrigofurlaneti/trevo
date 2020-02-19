using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;

namespace Dominio
{
    public interface IHistoricoArquivoServico : IBaseServico<HistoricoArquivo>
    {
    }

    public class HistoricoArquivoServico : BaseServico<HistoricoArquivo, IHistoricoArquivoRepositorio>, IHistoricoArquivoServico
    {
    }
}