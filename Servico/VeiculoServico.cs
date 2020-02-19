using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;


namespace Dominio
{
    public interface IVeiculoServico : IBaseServico<Veiculo>
    {

    }

    public class VeiculoServico : BaseServico<Veiculo, IVeiculoRepositorio>, IVeiculoServico
    {
    }
}
