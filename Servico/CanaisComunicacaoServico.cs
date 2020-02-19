using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;

namespace Dominio
{
    public interface ICanaisComunicacaoServico : IBaseServico<CanaisComunicacao>
    {
    }

    public class CanaisComunicacaoServico : BaseServico<CanaisComunicacao, ICanaisComunicacaoRepositorio>, ICanaisComunicacaoServico
    {
    }
}