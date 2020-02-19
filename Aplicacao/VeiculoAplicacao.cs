using Aplicacao.Base;
using Dominio;
using Entidade;
using System.Collections.Generic;

namespace Aplicacao
{
    public interface IVeiculoAplicacao : IBaseAplicacao<Veiculo>
    {
        IList<Veiculo> ListaVeiculo();
    }

    public class VeiculoAplicacao : BaseAplicacao<Veiculo, IVeiculoServico>, IVeiculoAplicacao
    {
        public readonly IVeiculoServico _veiculoServico;

        public VeiculoAplicacao(IVeiculoServico veiculoServico)
        {
            _veiculoServico = veiculoServico;
        }


        public IList<Veiculo> ListaVeiculo()
        {
            return _veiculoServico.Buscar();

        }
    }
}
