using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;

namespace Repositorio
{
    public class TabelaPrecoAvulsoNotificacaoRepositorio : NHibRepository<TabelaPrecoAvulsoNotificacao>, ITabelaPrecoAvulsoNotificacaoRepositorio
    {
        public TabelaPrecoAvulsoNotificacaoRepositorio(NHibContext context) 
            : base(context)
        {
        }
    }
}