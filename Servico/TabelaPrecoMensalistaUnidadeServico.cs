using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;

namespace Dominio
{
    public interface ITabelaPrecoMensalistaUnidadeServico : IBaseServico<TabelaPrecoMensalistaUnidade>
    {
    }

    public class TabelaPrecoMensalistaUnidadeServico : BaseServico<TabelaPrecoMensalistaUnidade, ITabelaPrecoMensalistaUnidadeRepositorio>, ITabelaPrecoMensalistaUnidadeServico
    {
    }
}