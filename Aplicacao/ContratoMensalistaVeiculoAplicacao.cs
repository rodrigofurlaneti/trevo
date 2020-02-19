using Aplicacao.Base;
using Dominio;
using Entidade;

namespace Aplicacao
{
    public interface IContratoMensalistaVeiculoAplicacao : IBaseAplicacao<ContratoMensalistaVeiculo>
    {
    }

    public class ContratoMensalistaVeiculoAplicacao : BaseAplicacao<ContratoMensalistaVeiculo, IContratoMensalistaVeiculoServico>, IContratoMensalistaVeiculoAplicacao
    {
    }
}