using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;

namespace Dominio
{
    public interface ILayoutServico : IBaseServico<Layout>
    {
    }

    public class LayoutServico : BaseServico<Layout, ILayoutRepositorio>, ILayoutServico
    {
    }
}