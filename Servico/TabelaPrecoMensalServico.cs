using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;

namespace Dominio
{
    public interface ITabelaPrecoMensalServico : IBaseServico<TabelaPrecoMensal>
    {

    }

    public class TabelaPrecoMensalServico : BaseServico<TabelaPrecoMensal, ITabelaPrecoMensalRepositorio>, ITabelaPrecoMensalServico
    {
    }
}
