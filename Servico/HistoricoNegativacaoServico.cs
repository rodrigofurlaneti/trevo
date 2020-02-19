using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;

namespace Dominio
{
    public interface IHistoricoNegativacaoServico : IBaseServico<HistoricoNegativacao>
    {
    }

    public class HistoricoNegativacaoServico : BaseServico<HistoricoNegativacao, IHistoricoNegativacaoRepositorio>, IHistoricoNegativacaoServico
    {
    }
}