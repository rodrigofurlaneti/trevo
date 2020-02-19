using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;

namespace Dominio
{
    public interface IClienteVeiculoServico : IBaseServico<ClienteVeiculo>
    {

    }

    public class ClienteVeiculoServico : BaseServico<ClienteVeiculo, IClienteVeiculoRepositorio>, IClienteVeiculoServico
    {
    }
}
