using Dominio.IRepositorio;
using Entidade;
using Repositorio.Base;

namespace Repositorio
{
    public class TokenRepositorio : NHibRepository<Token>, ITokenRepositorio
    {
        public TokenRepositorio(NHibContext context)
            : base(context)
        {
        }
    }
}