using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;

namespace Repositorio
{
    public class MovimentacaoSeloRepositorio : NHibRepository<MovimentacaoSelo>, IMovimentacaoSeloRepositorio
    {
        public MovimentacaoSeloRepositorio(NHibContext context)
            : base(context)
        {
        }
    }
}