using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;

namespace Dominio
{
    public interface ITipoAtividadeServico : IBaseServico<TipoAtividade>
    {
    }

    public class TipoAtividadeServico : BaseServico<TipoAtividade, ITipoAtividadeRepositorio>, ITipoAtividadeServico
    {
    }
}