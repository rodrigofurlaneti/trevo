using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;

namespace Repositorio
{
    public class TipoAtividadeRepositorio : NHibRepository<TipoAtividade>, ITipoAtividadeRepositorio
    {
        public TipoAtividadeRepositorio(NHibContext context)
            : base(context)
        {
        }
    }
}