using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;

namespace Repositorio
{
    public class ParametroEquipeNotificacaoRepositorio : NHibRepository<ParametroEquipeNotificacao>, IParametroEquipeNotificacaoRepositorio
    {
        public ParametroEquipeNotificacaoRepositorio(NHibContext context)
            : base(context)
        {
        }
    }
}