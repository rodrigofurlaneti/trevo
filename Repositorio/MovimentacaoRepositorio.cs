using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;

namespace Repositorio
{
    public class MovimentacaoRepositorio : NHibRepository<Movimentacao>, IMovimentacaoRepositorio
    {
        public MovimentacaoRepositorio(NHibContext context)
            : base(context)
        {
                
        }
    }
}
