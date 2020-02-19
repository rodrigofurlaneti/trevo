using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;

namespace Repositorio
{
    public class ContratoMensalistaVeiculoRepositorio : NHibRepository<ContratoMensalistaVeiculo>, IContratoMensalistaVeiculoRepositorio
    {
        public ContratoMensalistaVeiculoRepositorio(NHibContext context)
            : base(context)
        {

        }
    }
}