using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;

namespace Dominio
{
    public interface IMarcaServico : IBaseServico<Marca>
    {
    }

    public class MarcaServico : BaseServico<Marca, IMarcaRepositorio>, IMarcaServico
    {
    }
}