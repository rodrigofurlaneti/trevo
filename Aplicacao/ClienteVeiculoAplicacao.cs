using Aplicacao.Base;
using Dominio;
using Entidade;

namespace Aplicacao
{
    public interface IClienteVeiculoAplicacao : IBaseAplicacao<ClienteVeiculo>
    {

    }
   
    public class ClienteVeiculoAplicacao : BaseAplicacao<ClienteVeiculo, IClienteVeiculoServico>, IClienteVeiculoAplicacao
    {
    }
}
