using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;

namespace Dominio
{
    public interface IParametrosLayoutServico : IBaseServico<ParametrosLayout>
    {
    }

    public class ParametrosLayoutServico : BaseServico<ParametrosLayout, IParametrosLayoutRepositorio>, IParametrosLayoutServico
    {
    }
}