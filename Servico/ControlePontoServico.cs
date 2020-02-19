using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;

namespace Dominio
{
    public interface IControlePontoServico : IBaseServico<ControlePonto>
    {

    }

    public class ControlePontoServico : BaseServico<ControlePonto, IControlePontoRepositorio>, IControlePontoServico
    {

    }
}