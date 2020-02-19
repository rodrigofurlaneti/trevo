using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;


namespace Repositorio
{
    public class VeiculoRepositorio : NHibRepository<Veiculo>, IVeiculoRepositorio
    {
        public VeiculoRepositorio(NHibContext context) : base(context)
        {

        }
    }
}
