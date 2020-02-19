using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;

namespace Dominio
{
    public interface IControlePontoFeriasServico : IBaseServico<ControlePontoFerias>
    {

    }

    public class ControlePontoFeriasServico : BaseServico<ControlePontoFerias, IControlePontoFeriasRepositorio>, IControlePontoFeriasServico
    {

    }
}