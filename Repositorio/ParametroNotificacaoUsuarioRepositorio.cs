using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;

namespace Repositorio
{
    public class ParametroNotificacaoUsuarioRepositorio : NHibRepository<ParametroNotificacaoUsuario>, IParametroNotificacaoUsuarioRepositorio 
    {
        public ParametroNotificacaoUsuarioRepositorio(NHibContext context) : base(context)
        {

        }
    }
}
