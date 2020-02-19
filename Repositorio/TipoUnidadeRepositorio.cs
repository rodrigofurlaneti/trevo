using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;

namespace Repositorio
{
    public class TipoUnidadeRepositorio : NHibRepository<TipoUnidade>,ITipoUnidadeRepositorio
    {
        public TipoUnidadeRepositorio(NHibContext context) : base(context)
        {

        }
    }
}
