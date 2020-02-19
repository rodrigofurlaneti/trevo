using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;

namespace Repositorio
{
    public class TabelaPrecoMensalistaNotificacaoRepositorio : NHibRepository<TabelaPrecoMensalistaNotificacao>, ITabelaPrecoMensalistaNotificacaoRepositorio
    {
        public TabelaPrecoMensalistaNotificacaoRepositorio(NHibContext context)
            : base(context)
        {
        }
    }
}