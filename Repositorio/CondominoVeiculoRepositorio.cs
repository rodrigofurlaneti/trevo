using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;

namespace Repositorio
{
    public class CondominoVeiculoRepositorio : NHibRepository<CondominoVeiculo>, ICondominoVeiculoRepositorio
    {
        public CondominoVeiculoRepositorio(NHibContext context)
            : base(context)
        {

        }
    }
}