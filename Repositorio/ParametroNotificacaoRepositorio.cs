using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;

namespace Repositorio
{
    public class ParametroNotificacaoRepositorio : NHibRepository<ParametroNotificacao>, IParametroNotificacaoRepositorio 
    {
        public ParametroNotificacaoRepositorio(NHibContext context) : base(context)
        {

        }
    }
}
