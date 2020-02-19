using Aplicacao.Base;
using Dominio;
using Entidade;


namespace Aplicacao
{
    public interface ICondominoVeiculoAplicacao : IBaseAplicacao<CondominoVeiculo>
    {

    }


    public class CondominoVeiculoAplicacao : BaseAplicacao<CondominoVeiculo, ICondominoVeiculoServico>, ICondominoVeiculoAplicacao
    {

    }
}
