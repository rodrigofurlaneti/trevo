using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;

namespace Repositorio
{
    public class TipoFilialRepositorio : NHibRepository<TipoFilial>, ITipoFilialRepositorio
    {
        public TipoFilialRepositorio(NHibContext context) 
            : base(context)
        {
            
        }
    }
}
