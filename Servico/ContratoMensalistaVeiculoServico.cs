using Dominio.Base;
using Dominio.IRepositorio;
using Entidade;

namespace Dominio
{
    public interface IContratoMensalistaVeiculoServico : IBaseServico<ContratoMensalistaVeiculo>
    {

    }
    public class ContratoMensalistaVeiculoServico : BaseServico<ContratoMensalistaVeiculo, IContratoMensalistaVeiculoRepositorio>, IContratoMensalistaVeiculoServico
    {

    }
}