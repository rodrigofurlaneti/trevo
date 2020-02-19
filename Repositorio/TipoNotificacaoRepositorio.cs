using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;

namespace Repositorio
{
    public class TipoNotificacaoRepositorio : NHibRepository<TipoNotificacao>, ITipoNotificacaoRepositorio
    {
        public TipoNotificacaoRepositorio(NHibContext context)
            : base(context)
        {
        }
    }
}