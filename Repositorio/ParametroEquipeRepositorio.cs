using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;

namespace Repositorio
{
    public class ParametroEquipeRepositorio : NHibRepository<ParametroEquipe>, IParametroEquipeRepositorio
    {
        public ParametroEquipeRepositorio(NHibContext context) : base(context)
        {

        }
    }
}
