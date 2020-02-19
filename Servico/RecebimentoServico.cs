using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;

namespace Dominio
{
    public interface IRecebimentoServico : IBaseServico<Recebimento>
    {
    }

    public class RecebimentoServico : BaseServico<Recebimento, IRecebimentoRepositorio>, IRecebimentoServico
    {
    }
}