using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;

namespace Repositorio
{
    public class PrecoParametroSeloRepositorio : NHibRepository<PrecoParametroSelo>, IPrecoParametroSeloRepositorio
    {
        public PrecoParametroSeloRepositorio(NHibContext context) : base(context)
        {

        }
    }
}