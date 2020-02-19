using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;

namespace Repositorio
{
    public class ClienteVeiculoRepositorio : NHibRepository<ClienteVeiculo>, IClienteVeiculoRepositorio
    {
        public ClienteVeiculoRepositorio(NHibContext context)
            : base(context)
        {

        }
    }
}