using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;

namespace Repositorio
{
    public class CotacaoNotificacaoRepositorio : NHibRepository<CotacaoNotificacao>, ICotacaoNotificacaoRepositorio
    {
        public CotacaoNotificacaoRepositorio(NHibContext context)
            : base(context)
        {
        }
    }
}