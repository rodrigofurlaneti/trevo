using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;

namespace Dominio
{
    public interface IPaisServico : IBaseServico<Pais>
    {
    }

    public class PaisServico : BaseServico<Pais, IPaisRepositorio>, IPaisServico
    {
    }
}