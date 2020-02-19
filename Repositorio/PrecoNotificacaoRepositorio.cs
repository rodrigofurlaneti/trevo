using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;

namespace Repositorio
{
    public class PrecoNotificacaoRepositorio : NHibRepository<PrecoNotificacao>, IPrecoNotificacaoRepositorio 
    {
        public PrecoNotificacaoRepositorio(NHibContext context)
            : base(context)
        {
        }
    }
}