using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;

namespace Dominio
{
    public interface IContatoServico : IBaseServico<Contato>
    {
    }

    public class ContatoServico : BaseServico<Contato, IContatoRepositorio>, IContatoServico
    {
    }
}