using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;

namespace Repositorio
{
    public class NecessidadeCompraNotificacaoRepositorio : NHibRepository<NecessidadeCompraNotificacao>, INecessidadeCompraNotificacaoRepositorio
    {
        public NecessidadeCompraNotificacaoRepositorio(NHibContext context)
            : base(context)
        {
        }
    }
}