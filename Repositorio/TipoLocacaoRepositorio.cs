using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;

namespace Repositorio
{
    public class TipoLocacaoRepositorio : NHibRepository<TipoLocacao>, ITipoLocacaoRepositorio
    {
        public TipoLocacaoRepositorio(NHibContext context)
            : base(context)
        {
        }
    }
}