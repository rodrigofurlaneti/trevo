using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;

namespace Dominio
{
    public interface IControleFeriasServico : IBaseServico<ControleFerias>
    {

    }

    public class ControleFeriasServico : BaseServico<ControleFerias, IControleFeriasRepositorio>, IControleFeriasServico
    {

    }
}