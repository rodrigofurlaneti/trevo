using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;

namespace Dominio
{
    public interface ITabelaPrecoServico: IBaseServico<TabelaPreco>
    {

    }
    public class TabelaPrecoServico : BaseServico<TabelaPreco, ITabelaPrecoRepositorio>, ITabelaPrecoServico
    {
    }
}
