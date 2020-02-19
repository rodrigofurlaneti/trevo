using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;

namespace Repositorio
{
    public class ParametroNegociacaoRepositorio : NHibRepository<ParametroNegociacao>, IParametroNegociacaoRepositorio
    {
        public ParametroNegociacaoRepositorio(NHibContext context): base(context)
        {

        }
    }
}
