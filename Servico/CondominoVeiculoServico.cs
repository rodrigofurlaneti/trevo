using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;

namespace Dominio
{
    public interface ICondominoVeiculoServico : IBaseServico<CondominoVeiculo>
    {

    }

    public class CondominoVeiculoServico : BaseServico<CondominoVeiculo, ICondominoVeiculoRepositorio>, ICondominoVeiculoServico
    {
    }
}
